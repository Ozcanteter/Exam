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
using Exam.Participants;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Exam.Shared;

namespace Exam.Participants
{

    [Authorize(ExamPermissions.Participants.Default)]
    public class ParticipantsAppService : ApplicationService, IParticipantsAppService
    {
        protected IDistributedCache<ParticipantExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        protected IParticipantRepository _participantRepository;
        protected ParticipantManager _participantManager;
        protected IRepository<Challenge, Guid> _challengeRepository;
        protected IRepository<IdentityUser, Guid> _identityUserRepository;

        public ParticipantsAppService(IParticipantRepository participantRepository, ParticipantManager participantManager, IDistributedCache<ParticipantExcelDownloadTokenCacheItem, string> excelDownloadTokenCache, IRepository<Challenge, Guid> challengeRepository, IRepository<IdentityUser, Guid> identityUserRepository)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _participantRepository = participantRepository;
            _participantManager = participantManager; _challengeRepository = challengeRepository;
            _identityUserRepository = identityUserRepository;
        }

        public virtual async Task<PagedResultDto<ParticipantWithNavigationPropertiesDto>> GetListAsync(GetParticipantsInput input)
        {
            var totalCount = await _participantRepository.GetCountAsync(input.FilterText, input.IsActive, input.ChallengeId, input.IdentityUserId);
            var items = await _participantRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.IsActive, input.ChallengeId, input.IdentityUserId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ParticipantWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ParticipantWithNavigationProperties>, List<ParticipantWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<ParticipantWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<ParticipantWithNavigationProperties, ParticipantWithNavigationPropertiesDto>
                (await _participantRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<ParticipantDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Participant, ParticipantDto>(await _participantRepository.GetAsync(id));
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

        [Authorize(ExamPermissions.Participants.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _participantRepository.DeleteAsync(id);
        }

        [Authorize(ExamPermissions.Participants.Create)]
        public virtual async Task<ParticipantDto> CreateAsync(ParticipantCreateDto input)
        {
            if (input.ChallengeId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Challenge"]]);
            }

            var participant = await _participantManager.CreateAsync(
            input.ChallengeId, input.IdentityUserId, input.IsActive
            );

            return ObjectMapper.Map<Participant, ParticipantDto>(participant);
        }

        [Authorize(ExamPermissions.Participants.Edit)]
        public virtual async Task<ParticipantDto> UpdateAsync(Guid id, ParticipantUpdateDto input)
        {
            if (input.ChallengeId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Challenge"]]);
            }

            var participant = await _participantManager.UpdateAsync(
            id,
            input.ChallengeId, input.IdentityUserId, input.IsActive, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Participant, ParticipantDto>(participant);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ParticipantExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var participants = await _participantRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.IsActive);
            var items = participants.Select(item => new
            {
                IsActive = item.Participant.IsActive,

                Challenge = item.Challenge?.Name,
                IdentityUser = item.IdentityUser?.UserName,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Participants.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<Exam.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new ParticipantExcelDownloadTokenCacheItem { Token = token },
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