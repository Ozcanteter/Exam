using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Exam.ProgressEntries
{
    public interface IProgressEntryRepository : IRepository<ProgressEntry, Guid>
    {
        Task<ProgressEntryWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<ProgressEntryWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            double? valueMin = null,
            double? valueMax = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<ProgressEntry>> GetListAsync(
                    string? filterText = null,
                    double? valueMin = null,
                    double? valueMax = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            double? valueMin = null,
            double? valueMax = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default);
    }
}