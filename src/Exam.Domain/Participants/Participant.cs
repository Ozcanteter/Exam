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

namespace Exam.Participants
{
    public class Participant : FullAuditedAggregateRoot<Guid>
    {
        public virtual bool IsActive { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid? IdentityUserId { get; set; }

        protected Participant()
        {

        }

        public Participant(Guid id, Guid challengeId, Guid? identityUserId, bool isActive)
        {

            Id = id;
            IsActive = isActive;
            ChallengeId = challengeId;
            IdentityUserId = identityUserId;
        }

    }
}