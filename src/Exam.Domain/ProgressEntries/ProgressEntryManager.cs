using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Exam.ProgressEntries
{
    public class ProgressEntryManager : DomainService
    {
        protected IProgressEntryRepository _progressEntryRepository;

        public ProgressEntryManager(IProgressEntryRepository progressEntryRepository)
        {
            _progressEntryRepository = progressEntryRepository;
        }

        public virtual async Task<ProgressEntry> CreateAsync(
        Guid challengeId, Guid identityUserId, double value)
        {
            Check.NotNull(challengeId, nameof(challengeId));
            Check.NotNull(identityUserId, nameof(identityUserId));

            var progressEntry = new ProgressEntry(
             GuidGenerator.Create(),
             challengeId, identityUserId, value
             );

            return await _progressEntryRepository.InsertAsync(progressEntry);
        }

        public virtual async Task<ProgressEntry> UpdateAsync(
            Guid id,
            Guid challengeId, Guid identityUserId, double value, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(challengeId, nameof(challengeId));
            Check.NotNull(identityUserId, nameof(identityUserId));

            var progressEntry = await _progressEntryRepository.GetAsync(id);

            progressEntry.ChallengeId = challengeId;
            progressEntry.IdentityUserId = identityUserId;
            progressEntry.Value = value;

            progressEntry.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _progressEntryRepository.UpdateAsync(progressEntry);
        }

    }
}