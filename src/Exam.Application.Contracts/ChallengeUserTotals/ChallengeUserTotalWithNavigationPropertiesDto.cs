using Exam.Challenges;
using Volo.Abp.Identity;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace Exam.ChallengeUserTotals
{
    public class ChallengeUserTotalWithNavigationPropertiesDto
    {
        public ChallengeUserTotalDto ChallengeUserTotal { get; set; } = null!;

        public ChallengeDto Challenge { get; set; } = null!;
        public IdentityUserDto IdentityUser { get; set; } = null!;

    }
}