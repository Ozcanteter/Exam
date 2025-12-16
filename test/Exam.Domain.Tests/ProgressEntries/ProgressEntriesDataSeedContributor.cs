using Exam.Challenges;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Exam.ProgressEntries;

namespace Exam.ProgressEntries
{
    public class ProgressEntriesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IProgressEntryRepository _progressEntryRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ChallengesDataSeedContributor _challengesDataSeedContributor;

        public ProgressEntriesDataSeedContributor(IProgressEntryRepository progressEntryRepository, IUnitOfWorkManager unitOfWorkManager, ChallengesDataSeedContributor challengesDataSeedContributor)
        {
            _progressEntryRepository = progressEntryRepository;
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

            await _progressEntryRepository.InsertAsync(new ProgressEntry
            (
                id: Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5"),
                value: 1290641882,
                challengeId: Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5"),
                identityUserId: Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5")
            ));

            await _progressEntryRepository.InsertAsync(new ProgressEntry
            (
                id: Guid.Parse("7e2ea07d-d806-4298-8e19-6d30b1907694"),
                value: 692300991,
                challengeId: Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5"),
                identityUserId: Guid.Parse("f9b573a3-2c41-412e-a266-59d303f79bb5")
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}