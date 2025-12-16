using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Exam.ProgressEntries
{
    public abstract class ProgressEntriesAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IProgressEntriesAppService _progressEntriesAppService;
        private readonly IRepository<ProgressEntry, Guid> _progressEntryRepository;

        public ProgressEntriesAppServiceTests()
        {
            _progressEntriesAppService = GetRequiredService<IProgressEntriesAppService>();
            _progressEntryRepository = GetRequiredService<IRepository<ProgressEntry, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _progressEntriesAppService.GetListAsync(new GetProgressEntriesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.ProgressEntry.Id == Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5")).ShouldBe(true);
            result.Items.Any(x => x.ProgressEntry.Id == Guid.Parse("7e2ea07d-d806-4298-8e19-6d30b1907694")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _progressEntriesAppService.GetAsync(Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ProgressEntryCreateDto
            {
                Value = 950419367,
                ChallengeId = Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5"),
                IdentityUserId = Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5")
            };

            // Act
            var serviceResult = await _progressEntriesAppService.CreateAsync(input);

            // Assert
            var result = await _progressEntryRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Value.ShouldBe(950419367);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ProgressEntryUpdateDto()
            {
                Value = 990525808,
                ChallengeId = Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5"),
                IdentityUserId = Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5")
            };

            // Act
            var serviceResult = await _progressEntriesAppService.UpdateAsync(Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5"), input);

            // Assert
            var result = await _progressEntryRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Value.ShouldBe(990525808);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _progressEntriesAppService.DeleteAsync(Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5"));

            // Assert
            var result = await _progressEntryRepository.FindAsync(c => c.Id == Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5"));

            result.ShouldBeNull();
        }
    }
}