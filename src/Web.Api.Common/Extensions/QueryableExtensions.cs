using System;
using System.Linq;
using System.Linq.Expressions;

namespace Web.Api.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        public static IQueryable<T> PageBy<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> orderBy,
            int page, int pageSize, bool orderByDescending = true)
        {
            const int defaultPageNumber = 1;

            if (page <= 0)
            {
                page = defaultPageNumber;
            }

            query = orderByDescending
                ? Queryable.OrderByDescending<T, TKey>(query, orderBy)
                : Queryable.OrderBy<T, TKey>(query, orderBy);

            return query.Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
