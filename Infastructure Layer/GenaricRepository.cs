using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Infastructure_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer
{
    internal class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext appDbContext;
        private readonly DbSet<T> _dbSet;

        public GenaricRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            _dbSet = appDbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return appDbContext.Set<T>();
        }

        public IQueryable<T> GetByIdQueryable(int id)
        {
            return appDbContext.Set<T>().Where(x => x.Id == id);
        }

        public IQueryable<T> GetByCriteriaQueryable(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }


        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await appDbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> GetByCriteriaAsync(Expression<Func<T, bool>> expression)
        {
            return await appDbContext.Set<T>().Where(expression).FirstAsync();
        }

        public async Task addAsync(T item)
        {
            await appDbContext.Set<T>().AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await appDbContext.Set<T>().AddRangeAsync(entities);
        }

        public void Delete(T item)
        {
            appDbContext.Set<T>().Remove(item);
        }

        public void SaveInclude(T entity)
        {
            var existingEntity = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);

            if (existingEntity == null)
            {
                existingEntity = _dbSet.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
                if (existingEntity == null)
                {
                    throw new Exception($"Entity of type {typeof(T).Name} with Id {entity.Id} not found.");
                }
                _dbSet.Attach(existingEntity);
            }

            var entry = appDbContext.Entry(existingEntity);

            var primaryKey = entry.Metadata.FindPrimaryKey();

            if (primaryKey == null)
            {
                throw new InvalidOperationException(
                    $"Entity {typeof(T).Name} does not have a primary key defined.");
            }

            var keyNames = primaryKey.Properties
                                      .Select(p => p.Name)
                                      .ToList();

            foreach (var property in typeof(T).GetProperties())
            {
                if (entry.Metadata.FindProperty(property.Name) == null)
                    continue;

                if (keyNames.Contains(property.Name))
                    continue;

                var oldvalue = property.GetValue(existingEntity);
                var newvale = property.GetValue(entity);

                if (newvale != null && !object.Equals(oldvalue, newvale))
                {
                    property.SetValue(existingEntity, newvale);
                    entry.Property(property.Name).IsModified = true;
                }
                else
                {
                    entry.Property(property.Name).IsModified = false;
                }
            }
        }

        public void Update(T item)
        {
            appDbContext.Set<T>().Update(item);
        }

        public async Task<int> SaveChanges()
        {
            return await appDbContext.SaveChangesAsync();
        }
    }
}
