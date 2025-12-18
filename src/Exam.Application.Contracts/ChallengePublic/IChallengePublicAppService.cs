using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Exam.ChallengePublic
{
    public interface IChallengePublicAppService : IApplicationService
    {
        Task CreateAsync(ChallengePublicCreateDto input);
        Task ProgressEntryCreateAsync(ChallengePublicProgressEntryCreateDto input, Guid challengeId);
        Task<List<ChallengePublicLeaderboardDto>> GetLeaderboardAsync(Guid challengeId);
        Task<ChallengePublicActiveChallengeResponseDto> GetActiveChallengesAsync(Guid userId);
    }
}
