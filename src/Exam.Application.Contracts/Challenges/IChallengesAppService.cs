using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Exam.Shared;

namespace Exam.Challenges
{
    public interface IChallengesAppService : IApplicationService
    {

        Task<PagedResultDto<ChallengeDto>> GetListAsync(GetChallengesInput input);

        Task<ChallengeDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<ChallengeDto> CreateAsync(ChallengeCreateDto input);

        Task<ChallengeDto> UpdateAsync(Guid id, ChallengeUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ChallengeExcelDownloadDto input);

        Task<Exam.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}