using Volo.Abp.Application.Dtos;
using System;

namespace Exam.ProgressEntries
{
    public class ProgressEntryExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public double? ValueMin { get; set; }
        public double? ValueMax { get; set; }
        public Guid? ChallengeId { get; set; }
        public Guid? IdentityUserId { get; set; }

        public ProgressEntryExcelDownloadDto()
        {

        }
    }
}