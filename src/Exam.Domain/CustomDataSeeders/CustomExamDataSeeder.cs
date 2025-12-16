using Exam.Challenges;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace Exam.CustomDataSeeders
{
    public class CustomExamDataSeeder : IDataSeedContributor, ITransientDependency
    {
        protected IOptions<IdentityOptions> _identityOptions;
        protected ILogger<CustomExamDataSeeder> _logger;
        protected IIdentityUserRepository _identityUserRepository;
        protected ILookupNormalizer _lookupNormalizer;
        protected IGuidGenerator _guidGenerator;
        protected IdentityUserManager _identityUserManager;
        protected IChallengeRepository _challengeRepository;
        protected ChallengeManager _challengeManager;

        public CustomExamDataSeeder(IOptions<IdentityOptions> identityOptions, ILogger<CustomExamDataSeeder> logger,
            IIdentityUserRepository identityUserRepository, ILookupNormalizer lookupNormalizer,
            IGuidGenerator guidGenerator, IdentityUserManager identityUserManager,
            IChallengeRepository challengeRepository, ChallengeManager challengeManager)
        {
            _identityOptions = identityOptions;
            _logger = logger;
            _identityUserRepository = identityUserRepository;
            _lookupNormalizer = lookupNormalizer;
            _guidGenerator = guidGenerator;
            _identityUserManager = identityUserManager;
            _challengeRepository = challengeRepository;
            _challengeManager = challengeManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedForeignUsersAsync();
            await SeedChallengesAsync();
        }

        private async Task SeedForeignUsersAsync()
        {
            _logger.LogInformation("Seeding 20 Foreign Users...");

            var foreignUsers = new[]
            {
                ("Maria", "Garcia", "maria.garcia@example.com", "Password@123"),
                ("Juan", "Martinez", "juan.martinez@example.com", "Password@123"),
                ("Sofia", "Rodriguez", "sofia.rodriguez@example.com", "Password@123"),
                ("Carlos", "Lopez", "carlos.lopez@example.com", "Password@123"),
                ("Lucia", "Sanchez", "lucia.sanchez@example.com", "Password@123"),
                ("Pierre", "Dubois", "pierre.dubois@example.com", "Password@123"),
                ("Marie", "Laurent", "marie.laurent@example.com", "Password@123"),
                ("Jean", "Bernard", "jean.bernard@example.com", "Password@123"),
                ("Margot", "Moreau", "margot.moreau@example.com", "Password@123"),
                ("Luc", "David", "luc.david@example.com", "Password@123"),
                ("Anna", "Mueller", "anna.mueller@example.com", "Password@123"),
                ("Hans", "Schmidt", "hans.schmidt@example.com", "Password@123"),
                ("Petra", "Wagner", "petra.wagner@example.com", "Password@123"),
                ("Klaus", "Fischer", "klaus.fischer@example.com", "Password@123"),
                ("Ingrid", "Weber", "ingrid.weber@example.com", "Password@123"),
                ("Marco", "Rossi", "marco.rossi@example.com", "Password@123"),
                ("Giulia", "Bianchi", "giulia.bianchi@example.com", "Password@123"),
                ("Antonio", "Ferrari", "antonio.ferrari@example.com", "Password@123"),
                ("Francesca", "Gallo", "francesca.gallo@example.com", "Password@123"),
                ("Matteo", "Russo", "matteo.russo@example.com", "Password@123"),
            };

            foreach (var (firstName, lastName, email, password) in foreignUsers)
            {
                await SeedUserAsync(firstName, lastName, email, password, null);
            }
        }

        private async Task SeedUserAsync(string firstName, string lastName, string email, string password, Guid? tenantId = null)
        {
            Check.NotNullOrWhiteSpace(email, nameof(email));
            Check.NotNullOrWhiteSpace(password, nameof(password));

            await _identityOptions.SetAsync();

            string? userName = email.Split("@")[0];

            if (string.IsNullOrWhiteSpace(userName))
            {
                return;
            }
            var existingUser = await _identityUserRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName(userName));
            if (existingUser != null)
            {
                return;
            }
            
            var fullName = $"{firstName} {lastName}".Trim();
            var newUser = new IdentityUser(_guidGenerator.Create(), userName, email, tenantId)
            {
                Name = string.IsNullOrWhiteSpace(fullName) ? userName : fullName
            };

            await _identityUserManager.CreateAsync(newUser, password, validatePassword: false);
        }

        private async Task SeedChallengesAsync()
        {
            _logger.LogInformation("Seeding 5 Challenges...");

            var challenges = new[]
            {
                new { Name = "Walk 10,000 Steps Daily", Goal = 10000.0, Description = "Walk 10,000 steps per day" },
                new { Name = "Drink 8 Glasses of Water", Goal = 8.0, Description = "Drink 8 glasses of water daily" },
                new { Name = "Exercise 30 Minutes", Goal = 30.0, Description = "Exercise for at least 30 minutes per day" },
                new { Name = "Read 20 Pages", Goal = 20.0, Description = "Read 20 pages of a book daily" },
                new { Name = "Meditate 15 Minutes", Goal = 15.0, Description = "Meditate for 15 minutes per day" },
            };

            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(30);

            foreach (var challenge in challenges)
            {
                await SeedChallengeAsync(challenge.Name, startDate, endDate, challenge.Goal, true);
            }
        }

        private async Task SeedChallengeAsync(string name, DateTime startDate, DateTime endDate, double goal, bool isActive)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingChallenge = await _challengeRepository.FindAsync(c => c.Name == name);
            if (existingChallenge != null)
            {
                _logger.LogInformation($"Challenge '{name}' already exists.");
                return;
            }

            var challenge = await _challengeManager.CreateAsync(name, startDate, endDate, goal, isActive);
            _logger.LogInformation($"Challenge '{challenge.Name}' created successfully with goal: {challenge.Goal}");
        }
    }
}
