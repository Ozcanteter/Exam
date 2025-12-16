using Exam.Challenges;
using Volo.Abp.Identity;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace Exam.Participants
{
    public class ParticipantWithNavigationPropertiesDto
    {
        public ParticipantDto Participant { get; set; } = null!;

        public ChallengeDto Challenge { get; set; } = null!;
        public IdentityUserDto IdentityUser { get; set; } = null!;

    }
}