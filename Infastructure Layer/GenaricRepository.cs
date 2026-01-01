using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Infastructure_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public void DeleteRange(IEnumerable<T> entities)
        {
            appDbContext.Set<T>().RemoveRange(entities);
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

          

            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey()) continue;



                var oldValue = property.CurrentValue;
                var newValue = appDbContext.Entry(entity).Property(property.Metadata.Name).CurrentValue;

                if (newValue != null && !object.Equals(oldValue, newValue))
                {
                    property.CurrentValue = newValue;
                    property.IsModified = true;
                }
                else
                {
                    property.IsModified = false;
                }
            }
        }


        public void SaveInclude(T entity, params string[] includedProperties)
        {
            var localEntity = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);
            EntityEntry<T> entry;

            if (localEntity != null)
            {
                entry = appDbContext.Entry(localEntity);

                foreach (var propertyName in includedProperties)
                {
                    var newValue = appDbContext.Entry(entity).Property(propertyName).CurrentValue;

                    entry.Property(propertyName).CurrentValue = newValue;
                    entry.Property(propertyName).IsModified = true;
                }
            }
            else
            {
                _dbSet.Attach(entity);
                entry = appDbContext.Entry(entity);

                foreach (var propertyName in includedProperties)
                {
                    entry.Property(propertyName).IsModified = true;
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
