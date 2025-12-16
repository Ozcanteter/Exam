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

namespace Exam.ProgressEntries
{
    public class EfCoreProgressEntryRepository : EfCoreRepository<ExamDbContext, ProgressEntry, Guid>, IProgressEntryRepository
    {
        public EfCoreProgressEntryRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<ProgressEntryWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(progressEntry => new ProgressEntryWithNavigationProperties
                {
                    ProgressEntry = progressEntry,
                    Challenge = dbContext.Set<Challenge>().FirstOrDefault(c => c.Id == progressEntry.ChallengeId),
                    IdentityUser = dbContext.Set<IdentityUser>().FirstOrDefault(c => c.Id == progressEntry.IdentityUserId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<ProgressEntryWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            double? valueMin = null,
            double? valueMax = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, valueMin, valueMax, challengeId, identityUserId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ProgressEntryConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<ProgressEntryWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from progressEntry in (await GetDbSetAsync())
                   join challenge in (await GetDbContextAsync()).Set<Challenge>() on progressEntry.ChallengeId equals challenge.Id into challenges
                   from challenge in challenges.DefaultIfEmpty()
                   join identityUser in (await GetDbContextAsync()).Set<IdentityUser>() on progressEntry.IdentityUserId equals identityUser.Id into identityUsers
                   from identityUser in identityUsers.DefaultIfEmpty()
                   select new ProgressEntryWithNavigationProperties
                   {
                       ProgressEntry = progressEntry,
                       Challenge = challenge,
                       IdentityUser = identityUser
                   };
        }

        protected virtual IQueryable<ProgressEntryWithNavigationProperties> ApplyFilter(
            IQueryable<ProgressEntryWithNavigationProperties> query,
            string? filterText,
            double? valueMin = null,
            double? valueMax = null,
            Guid? challengeId = null,
            Guid? identityUserId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => true)
                    .WhereIf(valueMin.HasValue, e => e.ProgressEntry.Value >= valueMin!.Value)
                    .WhereIf(valueMax.HasValue, e => e.ProgressEntry.Value <= valueMax!.Value)
                    .WhereIf(challengeId != null && challengeId != Guid.Empty, e => e.Challenge != null && e.Challenge.Id == challengeId)
                    .WhereIf(identityUserId != null && identityUserId != Guid.Empty, e => e.IdentityUser != null && e.IdentityUser.Id == identityUserId);
        }

        public virtual async Task<List<ProgressEntry>> GetListAsync(
            string? filterText = null,
            double? valueMin = null,
            double? valueMax = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, valueMin, valueMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ProgressEntryConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            double? valueMin = null,
            double? valueMax = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, valueMin, valueMax, challengeId, identityUserId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ProgressEntry> ApplyFilter(
            IQueryable<ProgressEntry> query,
            string? filterText = null,
            double? valueMin = null,
            double? valueMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => true)
                    .WhereIf(valueMin.HasValue, e => e.Value >= valueMin!.Value)
                    .WhereIf(valueMax.HasValue, e => e.Value <= valueMax!.Value);
        }
    }
}