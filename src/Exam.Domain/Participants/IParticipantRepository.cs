using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Exam.Participants
{
    public interface IParticipantRepository : IRepository<Participant, Guid>
    {
        Task<ParticipantWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<ParticipantWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            bool? isActive = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<Participant>> GetListAsync(
                    string? filterText = null,
                    bool? isActive = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            bool? isActive = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default);
    }
}