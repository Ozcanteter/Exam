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

namespace Exam.ChallengeUserTotals
{
    public class EfCoreChallengeUserTotalRepository : EfCoreRepository<ExamDbContext, ChallengeUserTotal, Guid>, IChallengeUserTotalRepository
    {
        public EfCoreChallengeUserTotalRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<ChallengeUserTotalWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(challengeUserTotal => new ChallengeUserTotalWithNavigationProperties
                {
                    ChallengeUserTotal = challengeUserTotal,
                    Challenge = dbContext.Set<Challenge>().FirstOrDefault(c => c.Id == challengeUserTotal.ChallengeId),
                    IdentityUser = dbContext.Set<IdentityUser>().FirstOrDefault(c => c.Id == challengeUserTotal.IdentityUserId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<ChallengeUserTotalWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            double? totalValueMin = null,
            double? totalValueMax = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, totalValueMin, totalValueMax, challengeId, identityUserId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ChallengeUserTotalConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<ChallengeUserTotalWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from challengeUserTotal in (await GetDbSetAsync())
                   join challenge in (await GetDbContextAsync()).Set<Challenge>() on challengeUserTotal.ChallengeId equals challenge.Id into challenges
                   from challenge in challenges.DefaultIfEmpty()
                   join identityUser in (await GetDbContextAsync()).Set<IdentityUser>() on challengeUserTotal.IdentityUserId equals identityUser.Id into identityUsers
                   from identityUser in identityUsers.DefaultIfEmpty()
                   select new ChallengeUserTotalWithNavigationProperties
                   {
                       ChallengeUserTotal = challengeUserTotal,
                       Challenge = challenge,
                       IdentityUser = identityUser
                   };
        }

        protected virtual IQueryable<ChallengeUserTotalWithNavigationProperties> ApplyFilter(
            IQueryable<ChallengeUserTotalWithNavigationProperties> query,
            string? filterText,
            double? totalValueMin = null,
            double? totalValueMax = null,
            Guid? challengeId = null,
            Guid? identityUserId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => true)
                    .WhereIf(totalValueMin.HasValue, e => e.ChallengeUserTotal.TotalValue >= totalValueMin!.Value)
                    .WhereIf(totalValueMax.HasValue, e => e.ChallengeUserTotal.TotalValue <= totalValueMax!.Value)
                    .WhereIf(challengeId != null && challengeId != Guid.Empty, e => e.Challenge != null && e.Challenge.Id == challengeId)
                    .WhereIf(identityUserId != null && identityUserId != Guid.Empty, e => e.IdentityUser != null && e.IdentityUser.Id == identityUserId);
        }

        public virtual async Task<List<ChallengeUserTotal>> GetListAsync(
            string? filterText = null,
            double? totalValueMin = null,
            double? totalValueMax = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, totalValueMin, totalValueMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ChallengeUserTotalConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            double? totalValueMin = null,
            double? totalValueMax = null,
            Guid? challengeId = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, totalValueMin, totalValueMax, challengeId, identityUserId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ChallengeUserTotal> ApplyFilter(
            IQueryable<ChallengeUserTotal> query,
            string? filterText = null,
            double? totalValueMin = null,
            double? totalValueMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => true)
                    .WhereIf(totalValueMin.HasValue, e => e.TotalValue >= totalValueMin!.Value)
                    .WhereIf(totalValueMax.HasValue, e => e.TotalValue <= totalValueMax!.Value);
        }
    }
}