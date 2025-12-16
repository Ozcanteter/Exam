using Volo.Abp.Application.Dtos;
using System;

namespace Exam.Participants
{
    public class ParticipantExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public bool? IsActive { get; set; }
        public Guid? ChallengeId { get; set; }
        public Guid? IdentityUserId { get; set; }

        public ParticipantExcelDownloadDto()
        {

        }
    }
}