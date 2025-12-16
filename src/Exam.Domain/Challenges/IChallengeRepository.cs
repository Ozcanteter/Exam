using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Exam.Challenges
{
    public interface IChallengeRepository : IRepository<Challenge, Guid>
    {
        Task<List<Challenge>> GetListAsync(
            string? filterText = null,
            string? name = null,
            DateTime? startDateMin = null,
            DateTime? startDateMax = null,
            DateTime? endDateMin = null,
            DateTime? endDateMax = null,
            double? goalMin = null,
            double? goalMax = null,
            bool? isActive = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            DateTime? startDateMin = null,
            DateTime? startDateMax = null,
            DateTime? endDateMin = null,
            DateTime? endDateMax = null,
            double? goalMin = null,
            double? goalMax = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);
    }
}