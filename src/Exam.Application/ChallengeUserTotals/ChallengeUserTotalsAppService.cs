using Volo.Abp.Identity;
using Exam.Challenges;
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
using Exam.ChallengeUserTotals;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Exam.Shared;

namespace Exam.ChallengeUserTotals
{

    [Authorize(ExamPermissions.ChallengeUserTotals.Default)]
    public class ChallengeUserTotalsAppService : ApplicationService, IChallengeUserTotalsAppService
    {
        protected IDistributedCache<ChallengeUserTotalExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        protected IChallengeUserTotalRepository _challengeUserTotalRepository;
        protected ChallengeUserTotalManager _challengeUserTotalManager;
        protected IRepository<Challenge, Guid> _challengeRepository;
        protected IRepository<IdentityUser, Guid> _identityUserRepository;

        public ChallengeUserTotalsAppService(IChallengeUserTotalRepository challengeUserTotalRepository, ChallengeUserTotalManager challengeUserTotalManager, IDistributedCache<ChallengeUserTotalExcelDownloadTokenCacheItem, string> excelDownloadTokenCache, IRepository<Challenge, Guid> challengeRepository, IRepository<IdentityUser, Guid> identityUserRepository)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _challengeUserTotalRepository = challengeUserTotalRepository;
            _challengeUserTotalManager = challengeUserTotalManager; _challengeRepository = challengeRepository;
            _identityUserRepository = identityUserRepository;
        }

        public virtual async Task<PagedResultDto<ChallengeUserTotalWithNavigationPropertiesDto>> GetListAsync(GetChallengeUserTotalsInput input)
        {
            var totalCount = await _challengeUserTotalRepository.GetCountAsync(input.FilterText, input.TotalValueMin, input.TotalValueMax, input.ChallengeId, input.IdentityUserId);
            var items = await _challengeUserTotalRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.TotalValueMin, input.TotalValueMax, input.ChallengeId, input.IdentityUserId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ChallengeUserTotalWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ChallengeUserTotalWithNavigationProperties>, List<ChallengeUserTotalWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<ChallengeUserTotalWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<ChallengeUserTotalWithNavigationProperties, ChallengeUserTotalWithNavigationPropertiesDto>
                (await _challengeUserTotalRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<ChallengeUserTotalDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ChallengeUserTotal, ChallengeUserTotalDto>(await _challengeUserTotalRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetChallengeLookupAsync(LookupRequestDto input)
        {
            var query = (await _challengeRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Name != null &&
                         x.Name.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Challenge>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Challenge>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input)
        {
            var query = (await _identityUserRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.UserName != null &&
                         x.UserName.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<IdentityUser>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<IdentityUser>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(ExamPermissions.ChallengeUserTotals.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _challengeUserTotalRepository.DeleteAsync(id);
        }

        [Authorize(ExamPermissions.ChallengeUserTotals.Create)]
        public virtual async Task<ChallengeUserTotalDto> CreateAsync(ChallengeUserTotalCreateDto input)
        {
            if (input.ChallengeId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Challenge"]]);
            }
            if (input.IdentityUserId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["IdentityUser"]]);
            }

            var challengeUserTotal = await _challengeUserTotalManager.CreateAsync(
            input.ChallengeId, input.IdentityUserId, input.TotalValue
            );

            return ObjectMapper.Map<ChallengeUserTotal, ChallengeUserTotalDto>(challengeUserTotal);
        }

        [Authorize(ExamPermissions.ChallengeUserTotals.Edit)]
        public virtual async Task<ChallengeUserTotalDto> UpdateAsync(Guid id, ChallengeUserTotalUpdateDto input)
        {
            if (input.ChallengeId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Challenge"]]);
            }
            if (input.IdentityUserId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["IdentityUser"]]);
            }

            var challengeUserTotal = await _challengeUserTotalManager.UpdateAsync(
            id,
            input.ChallengeId, input.IdentityUserId, input.TotalValue, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<ChallengeUserTotal, ChallengeUserTotalDto>(challengeUserTotal);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ChallengeUserTotalExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var challengeUserTotals = await _challengeUserTotalRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.TotalValueMin, input.TotalValueMax);
            var items = challengeUserTotals.Select(item => new
            {
                TotalValue = item.ChallengeUserTotal.TotalValue,

                Challenge = item.Challenge?.Name,
                IdentityUser = item.IdentityUser?.UserName,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "ChallengeUserTotals.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<Exam.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new ChallengeUserTotalExcelDownloadTokenCacheItem { Token = token },
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