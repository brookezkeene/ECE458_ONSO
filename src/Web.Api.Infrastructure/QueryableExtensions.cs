﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Web.Api.Infrastructure
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
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);

            return query.Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
