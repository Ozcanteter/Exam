using Volo.Abp.Application.Dtos;
using System;

namespace Exam.ChallengeUserTotals
{
    public class ChallengeUserTotalExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public double? TotalValueMin { get; set; }
        public double? TotalValueMax { get; set; }
        public Guid? ChallengeId { get; set; }
        public Guid? IdentityUserId { get; set; }

        public ChallengeUserTotalExcelDownloadDto()
        {

        }
    }
}