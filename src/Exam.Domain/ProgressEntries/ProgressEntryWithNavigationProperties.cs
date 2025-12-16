using Exam.Challenges;
using Volo.Abp.Identity;

using System;
using System.Collections.Generic;

namespace Exam.ProgressEntries
{
    public  class ProgressEntryWithNavigationProperties
    {
        public ProgressEntry ProgressEntry { get; set; } = null!;

        public Challenge Challenge { get; set; } = null!;
        public IdentityUser IdentityUser { get; set; } = null!;
        

        
    }
}