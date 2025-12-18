using Exam.ChallengePublic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exam.Controllers.ChallengePublic
{

    public class ChallengePublicController : ExamController, IChallengePublicAppService
    {
        private readonly IChallengePublicAppService _challengePublicAppService;
        public ChallengePublicController(IChallengePublicAppService challengePublicAppService)
        {
            _challengePublicAppService = challengePublicAppService;
        }

        [HttpPost]
        [Route("api/challenges/create")]
        public virtual Task CreateAsync(ChallengePublicCreateDto input)
        {
            return _challengePublicAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("api/challenges/{challengeId}/progress")]
        public virtual Task ProgressEntryCreateAsync(ChallengePublicProgressEntryCreateDto input, Guid challengeId)
        {
            return _challengePublicAppService.ProgressEntryCreateAsync(input, challengeId);
        }

        [HttpGet]
        [Route("api/challenges/{challengeId}/leaderboard")]
        public virtual Task<List<ChallengePublicLeaderboardDto>> GetLeaderboardAsync(Guid challengeId)
        {
            return _challengePublicAppService.GetLeaderboardAsync(challengeId);
        }

        [HttpGet]
        [Route("api/users/{userId}/active")]
        public virtual Task<ChallengePublicActiveChallengeResponseDto> GetActiveChallengesAsync(Guid userId)
        {
            return _challengePublicAppService.GetActiveChallengesAsync(userId);
        }
    }
}
