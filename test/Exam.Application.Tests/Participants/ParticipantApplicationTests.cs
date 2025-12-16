using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Exam.Participants
{
    public abstract class ParticipantsAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IParticipantsAppService _participantsAppService;
        private readonly IRepository<Participant, Guid> _participantRepository;

        public ParticipantsAppServiceTests()
        {
            _participantsAppService = GetRequiredService<IParticipantsAppService>();
            _participantRepository = GetRequiredService<IRepository<Participant, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _participantsAppService.GetListAsync(new GetParticipantsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Participant.Id == Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3")).ShouldBe(true);
            result.Items.Any(x => x.Participant.Id == Guid.Parse("547ddc72-5e5d-4d51-b030-e49b4efcbb1e")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _participantsAppService.GetAsync(Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ParticipantCreateDto
            {
                IsActive = true,
                ChallengeId = Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"),

            };

            // Act
            var serviceResult = await _participantsAppService.CreateAsync(input);

            // Assert
            var result = await _participantRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IsActive.ShouldBe(true);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ParticipantUpdateDto()
            {
                IsActive = true,
                ChallengeId = Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"),

            };

            // Act
            var serviceResult = await _participantsAppService.UpdateAsync(Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"), input);

            // Assert
            var result = await _participantRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IsActive.ShouldBe(true);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _participantsAppService.DeleteAsync(Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"));

            // Assert
            var result = await _participantRepository.FindAsync(c => c.Id == Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"));

            result.ShouldBeNull();
        }
    }
}