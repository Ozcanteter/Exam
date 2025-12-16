using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Exam.ChallengeUserTotals
{
    public class ChallengeUserTotalManager : DomainService
    {
        protected IChallengeUserTotalRepository _challengeUserTotalRepository;

        public ChallengeUserTotalManager(IChallengeUserTotalRepository challengeUserTotalRepository)
        {
            _challengeUserTotalRepository = challengeUserTotalRepository;
        }

        public virtual async Task<ChallengeUserTotal> CreateAsync(
        Guid challengeId, Guid identityUserId, double totalValue)
        {
            Check.NotNull(challengeId, nameof(challengeId));
            Check.NotNull(identityUserId, nameof(identityUserId));

            var challengeUserTotal = new ChallengeUserTotal(
             GuidGenerator.Create(),
             challengeId, identityUserId, totalValue
             );

            return await _challengeUserTotalRepository.InsertAsync(challengeUserTotal);
        }

        public virtual async Task<ChallengeUserTotal> UpdateAsync(
            Guid id,
            Guid challengeId, Guid identityUserId, double totalValue, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(challengeId, nameof(challengeId));
            Check.NotNull(identityUserId, nameof(identityUserId));

            var challengeUserTotal = await _challengeUserTotalRepository.GetAsync(id);

            challengeUserTotal.ChallengeId = challengeId;
            challengeUserTotal.IdentityUserId = identityUserId;
            challengeUserTotal.TotalValue = totalValue;

            challengeUserTotal.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _challengeUserTotalRepository.UpdateAsync(challengeUserTotal);
        }

    }
}