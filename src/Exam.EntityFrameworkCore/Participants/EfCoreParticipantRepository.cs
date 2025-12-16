using Volo.Abp.Identity;
using Exam.Challenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Exam.EntityFrameworkCore;

namespace Exam.Participants
{
    public class EfCoreParticipantRepository : EfCoreRepository<ExamDbContext, Participant, Guid>, IParticipantRepository
    {
        public EfCoreParticipantRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<ParticipantWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(participant => new ParticipantWithNavigationProperties
                {
                    Participant = participant,
                    Challenge = dbContext.Set<Challenge>().FirstOrDefault(c => c.Id == participant.ChallengeId),
                    IdentityUser = dbContext.Set<IdentityUser>().FirstOrDefault(c => c.Id == participant.IdentityUserId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<ParticipantWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            bool? isActive = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, isActive, challengeId, identityUserId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ParticipantConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<ParticipantWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from participant in (await GetDbSetAsync())
                   join challenge in (await GetDbContextAsync()).Set<Challenge>() on participant.ChallengeId equals challenge.Id into challenges
                   from challenge in challenges.DefaultIfEmpty()
                   join identityUser in (await GetDbContextAsync()).Set<IdentityUser>() on participant.IdentityUserId equals identityUser.Id into identityUsers
                   from identityUser in identityUsers.DefaultIfEmpty()
                   select new ParticipantWithNavigationProperties
                   {
                       Participant = participant,
                       Challenge = challenge,
                       IdentityUser = identityUser
                   };
        }

        protected virtual IQueryable<ParticipantWithNavigationProperties> ApplyFilter(
            IQueryable<ParticipantWithNavigationProperties> query,
            string? filterText,
            bool? isActive = null,
            Guid? challengeId = null,
            Guid? identityUserId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => true)
                    .WhereIf(isActive.HasValue, e => e.Participant.IsActive == isActive)
                    .WhereIf(challengeId != null && challengeId != Guid.Empty, e => e.Challenge != null && e.Challenge.Id == challengeId)
                    .WhereIf(identityUserId != null && identityUserId != Guid.Empty, e => e.IdentityUser != null && e.IdentityUser.Id == identityUserId);
        }

        public virtual async Task<List<Participant>> GetListAsync(
            string? filterText = null,
            bool? isActive = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, isActive);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ParticipantConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            bool? isActive = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, isActive, challengeId, identityUserId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Participant> ApplyFilter(
            IQueryable<Participant> query,
            string? filterText = null,
            bool? isActive = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => true)
                    .WhereIf(isActive.HasValue, e => e.IsActive == isActive);
        }
    }
}