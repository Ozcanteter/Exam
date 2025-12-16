using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Exam.Participants;
using Exam.EntityFrameworkCore;
using Xunit;

namespace Exam.EntityFrameworkCore.Domains.Participants
{
    public class ParticipantRepositoryTests : ExamEntityFrameworkCoreTestBase
    {
        private readonly IParticipantRepository _participantRepository;

        public ParticipantRepositoryTests()
        {
            _participantRepository = GetRequiredService<IParticipantRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _participantRepository.GetListAsync(
                    isActive: true
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _participantRepository.GetCountAsync(
                    isActive: true
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}