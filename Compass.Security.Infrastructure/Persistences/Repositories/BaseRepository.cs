using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Commons.Interfaces;
using Compass.Security.Infrastructure.Persistences.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Compass.Security.Infrastructure.Persistences.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
    {
        private readonly ApplicationDbContext _context;

        protected BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> AnyAsync(Guid id)
        {
            return await _context
                .Set<T>()
                .AsNoTracking()
                .AnyAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _context
                .Set<T>()
                .AsNoTracking()
                .AnyAsync(filter);
        }
        
        public async Task<int> GetCountAsync(Guid id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .CountAsync(x => x.Id.Equals(id));
        }
        
        public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .CountAsync(filter);
        }
        
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _context
                .Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context
                .Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<T> GetFilterAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<T>> GetSearchAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(filter)
                .ToListAsync();
        }
        
        public async Task<T> GetFilterAsync(Expression<Func<T, bool>> filter, Expression<Func<T, bool>> order)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<T>> GetSearchAsync(Expression<Func<T, bool>> filter, Expression<Func<T, bool>> order)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(filter)
                .ToListAsync();
        }
        
        public async Task<T> InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        
        public async Task<T> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            
            _context.Entry(entity).State = EntityState.Detached;
            
            return entity;
        }
        
        public async Task<T> AttachAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return entity;
        }

        public async Task<T> DeleteAsync(Guid id)
        {
            var entity = await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            _context.Remove(entity);
            
            await _context.SaveChangesAsync();
            
            return entity;
        }
        
        public async Task<IEnumerable<T>> InsertBulkAsync(List<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<IEnumerable<T>> UpdateBulkAsync(List<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<IEnumerable<T>> DeleteBulkAsync(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }
    }
}