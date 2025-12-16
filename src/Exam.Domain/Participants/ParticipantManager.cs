using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Exam.Participants
{
    public class ParticipantManager : DomainService
    {
        protected IParticipantRepository _participantRepository;

        public ParticipantManager(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public virtual async Task<Participant> CreateAsync(
        Guid challengeId, Guid? identityUserId, bool isActive)
        {
            Check.NotNull(challengeId, nameof(challengeId));

            var participant = new Participant(
             GuidGenerator.Create(),
             challengeId, identityUserId, isActive
             );

            return await _participantRepository.InsertAsync(participant);
        }

        public virtual async Task<Participant> UpdateAsync(
            Guid id,
            Guid challengeId, Guid? identityUserId, bool isActive, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(challengeId, nameof(challengeId));

            var participant = await _participantRepository.GetAsync(id);

            participant.ChallengeId = challengeId;
            participant.IdentityUserId = identityUserId;
            participant.IsActive = isActive;

            participant.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _participantRepository.UpdateAsync(participant);
        }

    }
}