using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mihaylov.Common.Abstract.Databases
{
    public static class LinqExtensions
    {
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

        public class JoinWrapper<TLeft, TRight>
        {
            public TLeft Left { get; set; }

            public TRight Right { get; set; }
        }
    }
}
