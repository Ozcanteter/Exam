namespace Exam.ChallengePublic;
public class ChallengePublicLeaderboardDto
{
    public string UserName { get; set; } = null!;
    public string ChallengeName { get; set; } = null!;
    public double TotalProgress { get; set; }
    public ushort PlaceIndex { get; set; }
}
