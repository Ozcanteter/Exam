using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Exam.ChallengeUserTotals
{
    public abstract class ChallengeUserTotalsAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IChallengeUserTotalsAppService _challengeUserTotalsAppService;
        private readonly IRepository<ChallengeUserTotal, Guid> _challengeUserTotalRepository;

        public ChallengeUserTotalsAppServiceTests()
        {
            _challengeUserTotalsAppService = GetRequiredService<IChallengeUserTotalsAppService>();
            _challengeUserTotalRepository = GetRequiredService<IRepository<ChallengeUserTotal, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _challengeUserTotalsAppService.GetListAsync(new GetChallengeUserTotalsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.ChallengeUserTotal.Id == Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170")).ShouldBe(true);
            result.Items.Any(x => x.ChallengeUserTotal.Id == Guid.Parse("8c9e8f4c-1a5d-4708-bd93-3eb6a869eff5")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _challengeUserTotalsAppService.GetAsync(Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ChallengeUserTotalCreateDto
            {
                TotalValue = 2075608849,
                ChallengeId = Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170"),
                IdentityUserId = Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170")
            };

            // Act
            var serviceResult = await _challengeUserTotalsAppService.CreateAsync(input);

            // Assert
            var result = await _challengeUserTotalRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.TotalValue.ShouldBe(2075608849);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ChallengeUserTotalUpdateDto()
            {
                TotalValue = 1169045726,
                ChallengeId = Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170"),
                IdentityUserId = Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170")
            };

            // Act
            var serviceResult = await _challengeUserTotalsAppService.UpdateAsync(Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170"), input);

            // Assert
            var result = await _challengeUserTotalRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.TotalValue.ShouldBe(1169045726);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _challengeUserTotalsAppService.DeleteAsync(Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170"));

            // Assert
            var result = await _challengeUserTotalRepository.FindAsync(c => c.Id == Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170"));

            result.ShouldBeNull();
        }
    }
}