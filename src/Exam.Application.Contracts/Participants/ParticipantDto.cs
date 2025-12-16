using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Exam.Participants
{
    public class ParticipantDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public bool IsActive { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid? IdentityUserId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}