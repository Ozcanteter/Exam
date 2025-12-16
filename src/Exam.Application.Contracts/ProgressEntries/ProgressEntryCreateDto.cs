using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Exam.ProgressEntries
{
    public class ProgressEntryCreateDto
    {
        public double Value { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid IdentityUserId { get; set; }
    }
}