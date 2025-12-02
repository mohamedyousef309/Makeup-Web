using Domain_Layer.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces.Repositryinterfaces
{
    public interface IGenaricRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetByIdQueryable(int id);

        Task<T> GetByCriteriaAsync(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);


        public  Task<bool> ExistsAsync(int id);


        public  Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);



        public   Task<int> CountAsync();


        public   Task<int> CountAsync(Expression<Func<T, bool>> predicate);
       



        void SaveInclude(T entity);

        Task addAsync(T item);

        Task AddRangeAsync(IEnumerable<T> entities);


        void Update(T item);

        void Delete(T item);

        Task<int> SaveChanges();
    }
}
