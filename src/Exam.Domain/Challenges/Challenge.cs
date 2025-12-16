using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Exam.Challenges
{
    public class Challenge : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Name { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual double Goal { get; set; }

        public virtual bool IsActive { get; set; }

        protected Challenge()
        {

        }

        public Challenge(Guid id, string name, DateTime startDate, DateTime endDate, double goal, bool isActive)
        {

            Id = id;
            Check.NotNull(name, nameof(name));
            Check.Length(name, nameof(name), ChallengeConsts.NameMaxLength, 0);
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Goal = goal;
            IsActive = isActive;
        }

    }
}