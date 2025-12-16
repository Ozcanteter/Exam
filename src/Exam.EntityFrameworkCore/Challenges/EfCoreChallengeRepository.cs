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

namespace Exam.Challenges
{
    public class EfCoreChallengeRepository : EfCoreRepository<ExamDbContext, Challenge, Guid>, IChallengeRepository
    {
        public EfCoreChallengeRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<Challenge>> GetListAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, name, startDateMin, startDateMax, endDateMin, endDateMax, goalMin, goalMax, isActive);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ChallengeConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            DateTime? startDateMin = null,
            DateTime? startDateMax = null,
            DateTime? endDateMin = null,
            DateTime? endDateMax = null,
            double? goalMin = null,
            double? goalMax = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, name, startDateMin, startDateMax, endDateMin, endDateMax, goalMin, goalMax, isActive);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Challenge> ApplyFilter(
            IQueryable<Challenge> query,
            string? filterText = null,
            string? name = null,
            DateTime? startDateMin = null,
            DateTime? startDateMax = null,
            DateTime? endDateMin = null,
            DateTime? endDateMax = null,
            double? goalMin = null,
            double? goalMax = null,
            bool? isActive = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(startDateMin.HasValue, e => e.StartDate >= startDateMin!.Value)
                    .WhereIf(startDateMax.HasValue, e => e.StartDate <= startDateMax!.Value)
                    .WhereIf(endDateMin.HasValue, e => e.EndDate >= endDateMin!.Value)
                    .WhereIf(endDateMax.HasValue, e => e.EndDate <= endDateMax!.Value)
                    .WhereIf(goalMin.HasValue, e => e.Goal >= goalMin!.Value)
                    .WhereIf(goalMax.HasValue, e => e.Goal <= goalMax!.Value)
                    .WhereIf(isActive.HasValue, e => e.IsActive == isActive);
        }
    }
}