using System;
using System.Collections.Generic;
using Volo.Abp.Caching;

namespace Exam.ChallengePublic;


[CacheName("ChallengeLeaderboard")]
public class ChallengeLeaderboardCacheItem
{
    public Dictionary<Guid, double> Scores { get; set; } = new();
}
