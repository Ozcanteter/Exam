using Exam.Challenges;
using Volo.Abp.Identity;

using System;
using System.Collections.Generic;

namespace Exam.ChallengeUserTotals
{
    public  class ChallengeUserTotalWithNavigationProperties
    {
        public ChallengeUserTotal ChallengeUserTotal { get; set; } = null!;

        public Challenge Challenge { get; set; } = null!;
        public IdentityUser IdentityUser { get; set; } = null!;
        

        
    }
}