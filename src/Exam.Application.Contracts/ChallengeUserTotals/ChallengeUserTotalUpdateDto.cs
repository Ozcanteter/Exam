using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Exam.ChallengeUserTotals
{
    public class ChallengeUserTotalUpdateDto : IHasConcurrencyStamp
    {
        public double TotalValue { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid IdentityUserId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}