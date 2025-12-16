using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Exam.ChallengeUserTotals
{
    public class ChallengeUserTotalDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public double TotalValue { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid IdentityUserId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}