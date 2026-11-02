using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides LINQ extension methods
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Gets one page of a query, projected to <typeparamref name="Tout"/>, together with the paging info.
        /// </summary>
        /// <typeparam name="Tin">The entity type of the query.</typeparam>
        /// <typeparam name="Tout">The type of the returned rows.</typeparam>
        /// <param name="query">The query to page. It should already be ordered.</param>
        /// <param name="pagerRequest">The requested page and page size. When null or without a page size, all rows are returned.</param>
        /// <param name="project">Maps the paged query to the returned rows.</param>
        /// <param name="ct">The token to cancel the operation.</param>
        /// <returns>
        /// The rows of the page and the <see cref="Pager"/>. A page number outside the range is moved to the nearest valid page.
        /// An empty query gives an empty grid on page 1.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="project"/> is null and the query has rows.</exception>
        public static async Task<BaseGrid<Tout>> GetPageAsync<Tin, Tout>(this IQueryable<Tin> query, IPagerRequest pagerRequest,
                Func<IQueryable<Tin>, IQueryable<Tout>> project, CancellationToken ct = default) where Tin : class where Tout : class
        {
            int? pageSize = pagerRequest?.PageSize;

            int count = await query.CountAsync(ct).ConfigureAwait(false);
            if (count == 0)
            {
                return new BaseGrid<Tout>()
                {
                    Data = new List<Tout>(),
                    Pager = new Pager(1, pageSize, 0)
                };
            }

            int currentPage = pagerRequest?.Page ?? 1;

            if (pageSize.HasValue)
            {
                int lastPage = (count + pageSize.Value - 1) / pageSize.Value;
                currentPage = Math.Clamp(currentPage, 1, lastPage);

                query = query.Skip((currentPage - 1) * pageSize.Value)
                             .Take(pageSize.Value)
                             .AsQueryable();
            }

            if (project == null)
            {
                throw new ArgumentNullException("Project cannot be null");
            }

            var rows = await project(query).ToListAsync(ct).ConfigureAwait(false);

            return new BaseGrid<Tout>()
            {
                Data = rows,
                Pager = new Pager(currentPage, pageSize, count)
            };
        }

        /// <summary>
        /// Performs the specified action on each element of the sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="collection">The sequence whose elements the action is applied to.</param>
        /// <param name="action">The action to apply to each element.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        /// <summary>
        /// Performs a left outer join between a left IQueryable and a right IEnumerable by matching keys and projects
        /// results using the provided selector.
        /// </summary>
        /// <typeparam name="TLeft">Type of elements in the left sequence.</typeparam>
        /// <typeparam name="TRight">Type of elements in the right sequence.</typeparam>
        /// <typeparam name="TKey">Type of the key used to match left and right elements.</typeparam>
        /// <typeparam name="TResult">Type of the elements produced by the result selector.</typeparam>
        /// <param name="left">Left input sequence to join.</param>
        /// <param name="right">Right input sequence to join.</param>
        /// <param name="leftKey">Expression that selects the join key from left elements.</param>
        /// <param name="rightKey">Expression that selects the join key from right elements.</param>
        /// <param name="resultSelector">Projection applied to each matched pair via a <![CDATA[JoinWrapper<TLeft, TRight>]]>; Right is null when there is no
        /// matching right element.</param>
        /// <returns>An <![CDATA[IQueryable<TResult>]]> representing the projected results of the left outer join; execution is deferred and
        /// can be translated by the query provider.</returns>
        public static IQueryable<TResult> LeftOuterJoin<TLeft, TRight, TKey, TResult>(this IQueryable<TLeft> left,
            IEnumerable<TRight> right, Expression<Func<TLeft, TKey>> leftKey, Expression<Func<TRight, TKey>> rightKey,
            Expression<Func<JoinWrapper<TLeft, TRight>, TResult>> resultSelector)
        {
            var result = left.GroupJoin(right, leftKey, rightKey, (l, r) => new { l, r })
                             .SelectMany(o => o.r.DefaultIfEmpty(),
                                         (l, r) => new JoinWrapper<TLeft, TRight> { Left = l.l, Right = r })
                             .Select(resultSelector);
            return result;
        }

        /// <summary>
        /// Represents a pair of values from left and right sources, typically used to convey the result of a join
        /// operation.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left element in the pair.</typeparam>
        /// <typeparam name="TRight">The type of the right element in the pair.</typeparam>
        public class JoinWrapper<TLeft, TRight>
        {
            /// <summary> Gets or sets the left value. </summary>
            public TLeft Left { get; set; }

            /// <summary> Gets or sets the right value.</summary>
            public TRight Right { get; set; }
        }
    }
}
