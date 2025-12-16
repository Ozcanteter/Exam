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
using Exam.ProgressEntries;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Exam.Shared;

namespace Exam.ProgressEntries
{

    [Authorize(ExamPermissions.ProgressEntries.Default)]
    public class ProgressEntriesAppService : ApplicationService, IProgressEntriesAppService
    {
        protected IDistributedCache<ProgressEntryExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        protected IProgressEntryRepository _progressEntryRepository;
        protected ProgressEntryManager _progressEntryManager;
        protected IRepository<Challenge, Guid> _challengeRepository;
        protected IRepository<IdentityUser, Guid> _identityUserRepository;

        public ProgressEntriesAppService(IProgressEntryRepository progressEntryRepository, ProgressEntryManager progressEntryManager, IDistributedCache<ProgressEntryExcelDownloadTokenCacheItem, string> excelDownloadTokenCache, IRepository<Challenge, Guid> challengeRepository, IRepository<IdentityUser, Guid> identityUserRepository)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _progressEntryRepository = progressEntryRepository;
            _progressEntryManager = progressEntryManager; _challengeRepository = challengeRepository;
            _identityUserRepository = identityUserRepository;
        }

        public virtual async Task<PagedResultDto<ProgressEntryWithNavigationPropertiesDto>> GetListAsync(GetProgressEntriesInput input)
        {
            var totalCount = await _progressEntryRepository.GetCountAsync(input.FilterText, input.ValueMin, input.ValueMax, input.ChallengeId, input.IdentityUserId);
            var items = await _progressEntryRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.ValueMin, input.ValueMax, input.ChallengeId, input.IdentityUserId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ProgressEntryWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ProgressEntryWithNavigationProperties>, List<ProgressEntryWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<ProgressEntryWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<ProgressEntryWithNavigationProperties, ProgressEntryWithNavigationPropertiesDto>
                (await _progressEntryRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<ProgressEntryDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ProgressEntry, ProgressEntryDto>(await _progressEntryRepository.GetAsync(id));
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

        [Authorize(ExamPermissions.ProgressEntries.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _progressEntryRepository.DeleteAsync(id);
        }

        [Authorize(ExamPermissions.ProgressEntries.Create)]
        public virtual async Task<ProgressEntryDto> CreateAsync(ProgressEntryCreateDto input)
        {
            if (input.ChallengeId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Challenge"]]);
            }
            if (input.IdentityUserId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["IdentityUser"]]);
            }

            var progressEntry = await _progressEntryManager.CreateAsync(
            input.ChallengeId, input.IdentityUserId, input.Value
            );

            return ObjectMapper.Map<ProgressEntry, ProgressEntryDto>(progressEntry);
        }

        [Authorize(ExamPermissions.ProgressEntries.Edit)]
        public virtual async Task<ProgressEntryDto> UpdateAsync(Guid id, ProgressEntryUpdateDto input)
        {
            if (input.ChallengeId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Challenge"]]);
            }
            if (input.IdentityUserId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["IdentityUser"]]);
            }

            var progressEntry = await _progressEntryManager.UpdateAsync(
            id,
            input.ChallengeId, input.IdentityUserId, input.Value, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<ProgressEntry, ProgressEntryDto>(progressEntry);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProgressEntryExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var progressEntries = await _progressEntryRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.ValueMin, input.ValueMax);
            var items = progressEntries.Select(item => new
            {
                Value = item.ProgressEntry.Value,

                Challenge = item.Challenge?.Name,
                IdentityUser = item.IdentityUser?.UserName,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "ProgressEntries.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<Exam.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new ProgressEntryExcelDownloadTokenCacheItem { Token = token },
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