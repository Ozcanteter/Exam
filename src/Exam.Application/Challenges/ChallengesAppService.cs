using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Exam.Permissions;
using Exam.Challenges;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Exam.Shared;

namespace Exam.Challenges
{

    [Authorize(ExamPermissions.Challenges.Default)]
    public class ChallengesAppService : ApplicationService, IChallengesAppService
    {
        protected IDistributedCache<ChallengeExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        protected IChallengeRepository _challengeRepository;
        protected ChallengeManager _challengeManager;

        public ChallengesAppService(IChallengeRepository challengeRepository, ChallengeManager challengeManager, IDistributedCache<ChallengeExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _challengeRepository = challengeRepository;
            _challengeManager = challengeManager;
        }

        public virtual async Task<PagedResultDto<ChallengeDto>> GetListAsync(GetChallengesInput input)
        {
            var totalCount = await _challengeRepository.GetCountAsync(input.FilterText, input.Name, input.StartDateMin, input.StartDateMax, input.EndDateMin, input.EndDateMax, input.GoalMin, input.GoalMax, input.IsActive);
            var items = await _challengeRepository.GetListAsync(input.FilterText, input.Name, input.StartDateMin, input.StartDateMax, input.EndDateMin, input.EndDateMax, input.GoalMin, input.GoalMax, input.IsActive, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ChallengeDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Challenge>, List<ChallengeDto>>(items)
            };
        }

        public virtual async Task<ChallengeDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Challenge, ChallengeDto>(await _challengeRepository.GetAsync(id));
        }

        [Authorize(ExamPermissions.Challenges.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _challengeRepository.DeleteAsync(id);
        }

        [Authorize(ExamPermissions.Challenges.Create)]
        public virtual async Task<ChallengeDto> CreateAsync(ChallengeCreateDto input)
        {

            var challenge = await _challengeManager.CreateAsync(
            input.Name, input.StartDate, input.EndDate, input.Goal, input.IsActive
            );

            return ObjectMapper.Map<Challenge, ChallengeDto>(challenge);
        }

        [Authorize(ExamPermissions.Challenges.Edit)]
        public virtual async Task<ChallengeDto> UpdateAsync(Guid id, ChallengeUpdateDto input)
        {

            var challenge = await _challengeManager.UpdateAsync(
            id,
            input.Name, input.StartDate, input.EndDate, input.Goal, input.IsActive, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Challenge, ChallengeDto>(challenge);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ChallengeExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _challengeRepository.GetListAsync(input.FilterText, input.Name, input.StartDateMin, input.StartDateMax, input.EndDateMin, input.EndDateMax, input.GoalMin, input.GoalMax, input.IsActive);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Challenge>, List<ChallengeExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Challenges.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<Exam.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new ChallengeExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new Exam.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}