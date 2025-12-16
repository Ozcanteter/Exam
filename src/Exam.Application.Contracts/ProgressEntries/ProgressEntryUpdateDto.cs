using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Exam.ProgressEntries
{
    public class ProgressEntryUpdateDto : IHasConcurrencyStamp
    {
        public double Value { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid IdentityUserId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}