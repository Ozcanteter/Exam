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

namespace Exam.ProgressEntries
{
    public class ProgressEntry : FullAuditedAggregateRoot<Guid>
    {
        public virtual double Value { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid IdentityUserId { get; set; }

        protected ProgressEntry()
        {

        }

        public ProgressEntry(Guid id, Guid challengeId, Guid identityUserId, double value)
        {

            Id = id;
            Value = value;
            ChallengeId = challengeId;
            IdentityUserId = identityUserId;
        }

    }
}