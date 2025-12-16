namespace Exam.ChallengePublic;

public class ChallengePublicActiveChallengeResponseDto
{
    public string UserName { get; set; } = null!;
    public int TotalCount { get; set; }
    public ChallengePublicActiveChallengeDto[] ActiveChallenges { get; set; } = null!;
}
