using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Exam.Challenges
{
    public class ChallengeManager : DomainService
    {
        protected IChallengeRepository _challengeRepository;

        public ChallengeManager(IChallengeRepository challengeRepository)
        {
            _challengeRepository = challengeRepository;
        }

        public virtual async Task<Challenge> CreateAsync(
        string name, DateTime startDate, DateTime endDate, double goal, bool isActive)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), ChallengeConsts.NameMaxLength);
            Check.NotNull(startDate, nameof(startDate));
            Check.NotNull(endDate, nameof(endDate));

            var challenge = new Challenge(
             GuidGenerator.Create(),
             name, startDate, endDate, goal, isActive
             );

            return await _challengeRepository.InsertAsync(challenge);
        }

        public virtual async Task<Challenge> UpdateAsync(
            Guid id,
            string name, DateTime startDate, DateTime endDate, double goal, bool isActive, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), ChallengeConsts.NameMaxLength);
            Check.NotNull(startDate, nameof(startDate));
            Check.NotNull(endDate, nameof(endDate));

            var challenge = await _challengeRepository.GetAsync(id);

            challenge.Name = name;
            challenge.StartDate = startDate;
            challenge.EndDate = endDate;
            challenge.Goal = goal;
            challenge.IsActive = isActive;

            challenge.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _challengeRepository.UpdateAsync(challenge);
        }

    }
}