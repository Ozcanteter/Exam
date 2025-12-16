using Exam.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Exam.Shared;

namespace Exam.Participants
{
    public interface IParticipantsAppService : IApplicationService
    {

        Task<PagedResultDto<ParticipantWithNavigationPropertiesDto>> GetListAsync(GetParticipantsInput input);

        Task<ParticipantWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<ParticipantDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetChallengeLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<ParticipantDto> CreateAsync(ParticipantCreateDto input);

        Task<ParticipantDto> UpdateAsync(Guid id, ParticipantUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ParticipantExcelDownloadDto input);

        Task<Exam.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}