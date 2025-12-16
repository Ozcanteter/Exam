using Volo.Abp.Application.Dtos;
using System;

namespace Exam.Participants
{
    public class GetParticipantsInput : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public bool? IsActive { get; set; }
        public Guid? ChallengeId { get; set; }
        public Guid? IdentityUserId { get; set; }

        public GetParticipantsInput()
        {

        }
    }
}