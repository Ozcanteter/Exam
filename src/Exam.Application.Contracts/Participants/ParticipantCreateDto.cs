using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Exam.Participants
{
    public class ParticipantCreateDto
    {
        public bool IsActive { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid? IdentityUserId { get; set; }
    }
}