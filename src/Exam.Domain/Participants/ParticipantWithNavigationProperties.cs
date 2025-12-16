using Exam.Challenges;
using Volo.Abp.Identity;

using System;
using System.Collections.Generic;

namespace Exam.Participants
{
    public  class ParticipantWithNavigationProperties
    {
        public Participant Participant { get; set; } = null!;

        public Challenge Challenge { get; set; } = null!;
        public IdentityUser IdentityUser { get; set; } = null!;
        

        
    }
}