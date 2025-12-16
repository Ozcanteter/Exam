using Exam.Challenges;
using Volo.Abp.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Exam.ChallengeUserTotals
{
    public class ChallengeUserTotal : FullAuditedAggregateRoot<Guid>
    {
        public virtual double TotalValue { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid IdentityUserId { get; set; }

        protected ChallengeUserTotal()
        {

        }

        public ChallengeUserTotal(Guid id, Guid challengeId, Guid identityUserId, double totalValue)
        {

            Id = id;
            TotalValue = totalValue;
            ChallengeId = challengeId;
            IdentityUserId = identityUserId;
        }

    }
}