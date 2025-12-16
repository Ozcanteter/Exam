using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Exam.ChallengeUserTotals
{
    public class ChallengeUserTotalCreateDto
    {
        public double TotalValue { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid IdentityUserId { get; set; }
    }
}