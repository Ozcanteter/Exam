using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Exam.ProgressEntries
{
    public class ProgressEntryDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public double Value { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid IdentityUserId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}