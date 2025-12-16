using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Exam.ChallengeUserTotals
{
    public interface IChallengeUserTotalRepository : IRepository<ChallengeUserTotal, Guid>
    {
        Task<ChallengeUserTotalWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<ChallengeUserTotalWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            double? totalValueMin = null,
            double? totalValueMax = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<ChallengeUserTotal>> GetListAsync(
                    string? filterText = null,
                    double? totalValueMin = null,
                    double? totalValueMax = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            double? totalValueMin = null,
            double? totalValueMax = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default);
    }
}