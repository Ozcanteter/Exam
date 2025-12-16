using Exam.Challenges;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Exam.ChallengeUserTotals;

namespace Exam.ChallengeUserTotals
{
    public class ChallengeUserTotalsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IChallengeUserTotalRepository _challengeUserTotalRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ChallengesDataSeedContributor _challengesDataSeedContributor;

        public ChallengeUserTotalsDataSeedContributor(IChallengeUserTotalRepository challengeUserTotalRepository, IUnitOfWorkManager unitOfWorkManager, ChallengesDataSeedContributor challengesDataSeedContributor)
        {
            _challengeUserTotalRepository = challengeUserTotalRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _challengesDataSeedContributor = challengesDataSeedContributor;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _challengesDataSeedContributor.SeedAsync(context);

            await _challengeUserTotalRepository.InsertAsync(new ChallengeUserTotal
            (
                id: Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170"),
                totalValue: 586592408,
                challengeId: Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170"),
                identityUserId: Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170")
            ));

            await _challengeUserTotalRepository.InsertAsync(new ChallengeUserTotal
            (
                id: Guid.Parse("8c9e8f4c-1a5d-4708-bd93-3eb6a869eff5"),
                totalValue: 843724934,
                challengeId: Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170"),
                identityUserId: Guid.Parse("bb92cca5-4a06-4127-a7a0-61fa0bda5170")
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}