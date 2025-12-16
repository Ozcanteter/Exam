using Exam.Challenges;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Exam.Participants;

namespace Exam.Participants
{
    public class ParticipantsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IParticipantRepository _participantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ChallengesDataSeedContributor _challengesDataSeedContributor;

        public ParticipantsDataSeedContributor(IParticipantRepository participantRepository, IUnitOfWorkManager unitOfWorkManager, ChallengesDataSeedContributor challengesDataSeedContributor)
        {
            _participantRepository = participantRepository;
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

            await _participantRepository.InsertAsync(new Participant
            (
                id: Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"),
                isActive: true,
                challengeId: Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"),
                identityUserId: null
            ));

            await _participantRepository.InsertAsync(new Participant
            (
                id: Guid.Parse("547ddc72-5e5d-4d51-b030-e49b4efcbb1e"),
                isActive: true,
                challengeId: Guid.Parse("756dce3b-36bf-4cc6-9603-0c3accc95bb3"),
                identityUserId: null
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}