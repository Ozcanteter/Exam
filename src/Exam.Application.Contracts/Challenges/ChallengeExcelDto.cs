using System;

namespace Exam.Challenges
{
    public class ChallengeExcelDto
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Goal { get; set; }
        public bool IsActive { get; set; }
    }
}