using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides LINQ extension methods, including LeftOuterJoin which performs a left outer join between an
    /// <![CDATA[IQueryable<TLeft>]]> and an <![CDATA[IEnumerable<TRight>]]>, producing TResult via a selector expression.
    /// </summary>
    public static class LinqExtensions
    {
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
