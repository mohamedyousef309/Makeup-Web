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

        protected IQueryable<T> ApplySearch<T>( IQueryable<T> query, string? search, Expression<Func<T, string>> propertySelector)
        {
            if (string.IsNullOrWhiteSpace(search))
                return query;

            var term = search.Trim().ToLower();

            // نقوم بتركيب Expression Tree يدوياً لضمان أن EF Core يفهمها
            // النتيجة ستكون مثل: x => x.Property.ToLower().Contains(term)

            var parameter = propertySelector.Parameters[0];
            var property = propertySelector.Body;

            // .ToLower()
            var toLowerMethod = typeof(string).GetMethod("ToLower", System.Type.EmptyTypes);
            var toLowerExpression = Expression.Call(property, toLowerMethod!);

            // .Contains(term)
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var searchTerm = Expression.Constant(term);
            var containsExpression = Expression.Call(toLowerExpression, containsMethod!, searchTerm);

            var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

            return query.Where(lambda);
        }

        protected IQueryable<T> ApplySorting<T>( IQueryable<T> query, string? sortBy, string? sortDir,
         Dictionary<string, Expression<Func<T, object>>> sortColumns) // نمرر الخريطة هنا
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
