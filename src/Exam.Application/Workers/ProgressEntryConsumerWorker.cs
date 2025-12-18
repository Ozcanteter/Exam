using Exam.ChallengePublic;
using Exam.Challenges;
using Exam.ChallengeUserTotals;
using Exam.Etos;
using Exam.Participants;
using Exam.ProgressEntries;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Exam.Workers;

public class ProgressEntryConsumerWorker(IConfiguration configuration, IChallengeRepository challengeRepository,
    IParticipantRepository participantRepository, IChallengeUserTotalRepository challengeUserTotalRepository,
    IProgressEntryRepository progressEntryRepository, ChallengeUserTotalManager challengeUserTotalManager,
    ProgressEntryManager progressEntryManager, ParticipantManager participantManager,
    IDistributedCache<ChallengeLeaderboardCacheItem, Guid> leaderboardCache
)
        : BackgroundWorkerBase, ISingletonDependency
{
    private readonly IConfiguration _configuration = configuration;
    private IConnection _connection;
    private IModel _channel;

    protected IChallengeRepository _challengeRepository = challengeRepository;

    protected IParticipantRepository _participantRepository = participantRepository;
    protected ParticipantManager _participantManager = participantManager;

    protected IChallengeUserTotalRepository _challengeUserTotalRepository = challengeUserTotalRepository;
    protected ChallengeUserTotalManager _challengeUserTotalManager = challengeUserTotalManager;

    protected IProgressEntryRepository _progressEntryRepository = progressEntryRepository;
    protected ProgressEntryManager _progressEntryManager = progressEntryManager;

    private readonly IDistributedCache<ChallengeLeaderboardCacheItem, Guid> _leaderboardCache = leaderboardCache;

    public override Task StartAsync(CancellationToken cancellationToken = default)
    {
        var rabbitSection = _configuration.GetSection("RabbitMQ");
        var connectionSection = rabbitSection
            .GetSection("Connections")
            .GetSection("Default");

        var exchangeName = rabbitSection
            .GetSection("EventBus")
            .GetValue<string>("ExchangeName");

        var factory = new ConnectionFactory
        {
            HostName = connectionSection.GetValue<string>("HostName"),
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: exchangeName,
            type: ExchangeType.Direct,
            durable: true
        );

        // ABP routing key convention:
        // fully-qualified event name OR short name
        var routingKey = typeof(ProgressEntryCreateEto).FullName!;

        var queueName = $"exam.progress-entry.{Environment.MachineName}";

        _channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        _channel.QueueBind(
            queue: queueName,
            exchange: exchangeName,
            routingKey: routingKey
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (_, args) =>
        {
            using var scope = ServiceProvider.CreateScope();

            try
            {
                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var eto = JsonSerializer.Deserialize<ProgressEntryCreateEto>(json);

                Logger.LogInformation(
                    "ProgressEntryCreateEto received at {Time}",
                    DateTimeOffset.Now
                );

                await ProcessAsync(scope.ServiceProvider, eto!);

                _channel.BasicAck(args.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while processing ProgressEntryCreateEto");
                _channel.BasicNack(args.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: consumer
        );

        Logger.LogInformation("ProgressEntryConsumerWorker started.");

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken = default)
    {
        _channel?.Close();
        _connection?.Close();

        Logger.LogInformation("ProgressEntryConsumerWorker stopped.");

        return Task.CompletedTask;
    }

    private async Task ProcessAsync(IServiceProvider serviceProvider, ProgressEntryCreateEto eto)
    {
        var challenge = await _challengeRepository.GetAsync(eto.ChallengeId);
        var participant = await _participantRepository.FindAsync(c => c.ChallengeId == eto.ChallengeId && c.IdentityUserId == eto.UserId);
        participant ??= await _participantManager.CreateAsync(challenge.Id, eto.UserId, isActive: true);

        await _progressEntryManager.CreateAsync(challenge.Id, eto.UserId, eto.Value);

        var userTotal = await _challengeUserTotalRepository.FindAsync(c => c.ChallengeId == challenge.Id && c.IdentityUserId == eto.UserId);
        if (userTotal == null)
        {
            userTotal = await _challengeUserTotalManager.CreateAsync(challenge.Id, eto.UserId, eto.Value);
        }
        else
        {
            userTotal.TotalValue += eto.Value;
            await _challengeUserTotalManager.UpdateAsync(userTotal.Id, userTotal.ChallengeId, userTotal.IdentityUserId, userTotal.TotalValue, userTotal.ConcurrencyStamp);
        }

        //update redis leaderboard

        // Redis leaderboard key
        var leaderboardKey = $"leaderboard:challenge:{challenge.Id}";

        var leaderboard = await _leaderboardCache.GetOrAddAsync(eto.ChallengeId, () => Task.FromResult(new ChallengeLeaderboardCacheItem()), () => new DistributedCacheEntryOptions
        { SlidingExpiration = TimeSpan.FromHours(1) });

        if (leaderboard!.Scores.ContainsKey(eto.UserId))
        {
            leaderboard.Scores[eto.UserId] += eto.Value;
        }
        else
        {
            leaderboard.Scores[eto.UserId] = eto.Value;
        }

        await _leaderboardCache.SetAsync(eto.ChallengeId, leaderboard);

    }
}
