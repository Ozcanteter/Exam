using Exam.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Exam.Shared;

namespace Exam.ChallengeUserTotals
{
    public interface IChallengeUserTotalsAppService : IApplicationService
    {

        Task<PagedResultDto<ChallengeUserTotalWithNavigationPropertiesDto>> GetListAsync(GetChallengeUserTotalsInput input);

        Task<ChallengeUserTotalWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<ChallengeUserTotalDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetChallengeLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<ChallengeUserTotalDto> CreateAsync(ChallengeUserTotalCreateDto input);

        Task<ChallengeUserTotalDto> UpdateAsync(Guid id, ChallengeUserTotalUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ChallengeUserTotalExcelDownloadDto input);

        Task<Exam.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}