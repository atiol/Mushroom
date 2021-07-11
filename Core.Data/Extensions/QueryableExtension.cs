using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Common.GenericSearch;
using Core.Common.Models.Grid;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Extensions
{
    public static class QueryableExtension
    {
        /// <summary>
        /// Converts the specified source to <see cref="GridResult{T}"/> by the specified <paramref name="pageIndex"/> and <paramref name="pageSize"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="query">The source to paging.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>An instance of the <see cref="GridResult{T}"/> class.</returns>
        public static async Task<GridResult<T>> ToGridResult<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var count = await query.CountAsync().ConfigureAwait(false);

            var items = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync()
                .ConfigureAwait(false);

            var pagedList = new GridResult<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = count,
                Items = items
            };

            return pagedList;
        }
        
        /// <summary>
        /// Apply the specified <paramref name="baseSearches"/> to <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="query">The source to apply base searches.</param>
        /// <param name="baseSearches">The list of the searches.</param>
        /// <returns>An instance of the <see cref="IQueryable{T}"/></returns>
        public static IQueryable<T> ApplyBaseSearchList<T>(this IQueryable<T> query, IEnumerable<BaseSearch> baseSearches)
        {
            return baseSearches.Aggregate(query, (current, criteria) => criteria.ApplyToQuery(current));
        }
        
        /// <summary>
        /// Apply the specified <paramref name="sort"/> to <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="query">The source to apply sort.</param>
        /// <param name="sort">The sort.</param>
        /// <returns>An instance of the <see cref="IQueryable{T}"/></returns>
        public static IQueryable<T> ApplySortFilter<T>(this IQueryable<T> query, Sort sort)
        {
            query = sort.ApplyToQuery(query);
            return query;
        }
    }
}