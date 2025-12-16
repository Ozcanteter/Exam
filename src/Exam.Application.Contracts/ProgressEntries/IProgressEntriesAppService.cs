using Exam.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Exam.Shared;

namespace Exam.ProgressEntries
{
    public interface IProgressEntriesAppService : IApplicationService
    {

        Task<PagedResultDto<ProgressEntryWithNavigationPropertiesDto>> GetListAsync(GetProgressEntriesInput input);

        Task<ProgressEntryWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<ProgressEntryDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetChallengeLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<ProgressEntryDto> CreateAsync(ProgressEntryCreateDto input);

        Task<ProgressEntryDto> UpdateAsync(Guid id, ProgressEntryUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProgressEntryExcelDownloadDto input);

        Task<Exam.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}