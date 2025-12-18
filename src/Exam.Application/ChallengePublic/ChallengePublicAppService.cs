using Exam.Challenges;
using Exam.ChallengeUserTotals;
using Exam.Etos;
using Exam.Participants;
using Exam.ProgressEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;

namespace Exam.ChallengePublic
{
    public class ChallengePublicAppService(IChallengeRepository challengeRepository, ChallengeManager challengeManager,
        IIdentityUserRepository identityUserRepository, IParticipantRepository participantRepository, ParticipantManager participantManager,
        IChallengeUserTotalRepository challengeUserTotalRepository, ChallengeUserTotalManager challengeUserTotalManager,
        ProgressEntryManager progressEntryManager, IProgressEntryRepository progressEntryRepository,
        IDistributedEventBus distributedEventBus, IDistributedCache<ChallengeLeaderboardCacheItem, Guid> leaderboardCache) : ExamAppService, IChallengePublicAppService
    {
        protected IChallengeRepository _challengeRepository = challengeRepository;
        protected ChallengeManager _challengeManager = challengeManager;

        protected IIdentityUserRepository _identityUserRepository = identityUserRepository;

        protected IParticipantRepository _participantRepository = participantRepository;
        protected ParticipantManager _participantManager = participantManager;

        protected IChallengeUserTotalRepository _challengeUserTotalRepository = challengeUserTotalRepository;
        protected ChallengeUserTotalManager _challengeUserTotalManager = challengeUserTotalManager;

        protected IProgressEntryRepository _progressEntryRepository = progressEntryRepository;
        protected ProgressEntryManager _progressEntryManager = progressEntryManager;

        protected IDistributedEventBus _distributedEventBus = distributedEventBus;

        protected IDistributedCache<ChallengeLeaderboardCacheItem, Guid> _leaderboardCache = leaderboardCache;

        public virtual async Task CreateAsync(ChallengePublicCreateDto input)
        {
            if (input.StartDate > input.EndDate)
                throw new UserFriendlyException(L["StartDateGreaterThanEndDate"]);

            await _challengeManager.CreateAsync(input.Name, input.StartDate, input.EndDate, input.Goal, true);
        }

        public virtual async Task<ChallengePublicActiveChallengeResponseDto> GetActiveChallengesAsync(Guid userId)
        {
            var checkUser = await _identityUserRepository.FindAsync(userId)
                ?? throw new UserFriendlyException(L["UserNotFound"]);

            var activeChallenges = await _participantRepository.GetListWithNavigationPropertiesAsync(identityUserId: userId, isActive: true);

            var result = new ChallengePublicActiveChallengeResponseDto
            {
                UserName = checkUser.UserName,
                TotalCount = activeChallenges.Count,
                ActiveChallenges = []
            };

            foreach (var participant in activeChallenges)
            {
                var total = await _challengeUserTotalRepository.FindAsync(c => c.ChallengeId == participant.Challenge.Id && c.IdentityUserId == userId);
                result.ActiveChallenges.Add(new ChallengePublicActiveChallengeDto
                {
                    ChallengeId = participant.Challenge.Id,
                    ChallengeName = participant.Challenge.Name,
                    TotalProgress = total != null ? total.TotalValue : 0
                });
            }

            return result;
        }

        public virtual async Task<List<ChallengePublicLeaderboardDto>> GetLeaderboardAsync(Guid challengeId)
        {
            var challenge = await _challengeRepository.FindAsync(challengeId) ?? throw new UserFriendlyException(L["ChallengeNotFound"]);

            var leaderboardCache = await _leaderboardCache.GetAsync(challengeId);

            if (leaderboardCache == null || leaderboardCache.Scores.Count == 0)
            {
                return [];
            }

            var topUsers = leaderboardCache.Scores
                .OrderByDescending(x => x.Value)
                .Take(10)
                .ToList();

            var leaderboard = new List<ChallengePublicLeaderboardDto>();
            ushort index = 0;

            foreach (var item in topUsers)
            {
                var user = await _identityUserRepository.GetAsync(item.Key);

                leaderboard.Add(new ChallengePublicLeaderboardDto
                {
                    UserName = user.UserName,
                    ChallengeName = challenge.Name,
                    TotalProgress = item.Value,
                    PlaceIndex = index++
                });
            }

            return leaderboard;
        }

        public virtual async Task ProgressEntryCreateAsync(ChallengePublicProgressEntryCreateDto input, Guid challengeId)
        {
            _ = await _challengeRepository.FindAsync(challengeId)
                ?? throw new UserFriendlyException(L["ChallengeNotFound"]);

            await _distributedEventBus.PublishAsync(new ProgressEntryCreateEto() { ChallengeId = challengeId, UserId = input.UserId, Value = input.Value }, useOutbox: true);
        }
    }
}
