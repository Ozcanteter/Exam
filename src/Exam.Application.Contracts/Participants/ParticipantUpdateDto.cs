using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Exam.Participants
{
    public class ParticipantUpdateDto : IHasConcurrencyStamp
    {
        public bool IsActive { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid? IdentityUserId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}