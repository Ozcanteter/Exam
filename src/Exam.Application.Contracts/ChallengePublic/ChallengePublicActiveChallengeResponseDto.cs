using System.Collections.Generic;

namespace Exam.ChallengePublic;

public class ChallengePublicActiveChallengeResponseDto
{
    public string UserName { get; set; } = null!;
    public int TotalCount { get; set; }
    public List<ChallengePublicActiveChallengeDto> ActiveChallenges { get; set; } = null!;
}
