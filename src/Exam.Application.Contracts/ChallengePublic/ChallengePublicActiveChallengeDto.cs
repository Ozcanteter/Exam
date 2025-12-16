using System;
namespace Exam.ChallengePublic;

public class ChallengePublicActiveChallengeDto
{
    public Guid ChallengeId { get; set; }
    public string ChallengeName { get; set; } = null!;
    public double TotalProgress { get; set; }
}