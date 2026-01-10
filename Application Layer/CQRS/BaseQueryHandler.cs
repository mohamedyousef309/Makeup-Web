using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application_Layer.CQRS
{
    public class BaseQueryHandler
    {
        protected void verifypagination(int pagenumber, int pagesize)
        {
            if (pagenumber <= 0)
                throw new ArgumentException("page number must be greater than 0.");
            if (pagesize <= 0 || pagesize > 100)
                throw new ArgumentException("page size must be between 1 and 100.");


        }


        protected IQueryable<T> ApplayPagination<T>(IQueryable<T> query, int pageNumber=1, int pageSize=7) 
        {
            verifypagination(pageNumber, pageSize);
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        }

        protected IQueryable<T> ApplySearch<T>(IQueryable<T> query, string? search, params Expression<Func<T, string>>[] propertySelectors)
        {
            if (string.IsNullOrWhiteSpace(search) || propertySelectors == null || propertySelectors.Length == 0)
                return query;

            var term = search.Trim().ToLower();
            var parameter = propertySelectors[0].Parameters[0];
            Expression? aggregateExpression = null;

            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var searchTerm = Expression.Constant(term);

            foreach (var selector in propertySelectors)
            {
                // استخراج الحقل (Property)
                var property = selector.Body;

                // x.Property.ToLower().Contains(term)
                var toLowerCall = Expression.Call(property, toLowerMethod!);
                var containsCall = Expression.Call(toLowerCall, containsMethod!, searchTerm);

                // دمج الشروط بـ OR
                aggregateExpression = aggregateExpression == null
                    ? containsCall
                    : Expression.OrElse(aggregateExpression, containsCall);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(aggregateExpression!, parameter);
            return query.Where(lambda);
        }

        protected IQueryable<T> ApplySorting<T>( IQueryable<T> query, string? sortBy, string? sortDir,
         Dictionary<string, Expression<Func<T, object>>> sortColumns) 
        {
            if (string.IsNullOrWhiteSpace(sortBy) || !sortColumns.ContainsKey(sortBy.ToLower()))
                return query;

            bool isAscending = string.IsNullOrEmpty(sortDir) || sortDir.ToLower() == "asc";

            var selectedColumn = sortColumns[sortBy.ToLower()];

            return isAscending
                ? query.OrderBy(selectedColumn)
                : query.OrderByDescending(selectedColumn);
        }


    }
}
