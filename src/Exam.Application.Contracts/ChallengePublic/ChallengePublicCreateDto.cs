using System;
namespace Exam.ChallengePublic;

public class ChallengePublicCreateDto
{
    public string Name { get; set; } = null!;
    public double Goal { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
