using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Exam.Challenges
{
    public class ChallengeCreateDto
    {
        [Required]
        [StringLength(ChallengeConsts.NameMaxLength)]
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Goal { get; set; }
        public bool IsActive { get; set; }
    }
}