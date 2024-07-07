using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity<int>
    {
        private ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
          var entity =await _context.Set<T>().FindAsync(id);
          _context.Set<T>().Remove(entity);
          await _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll() => _context.Set<T>().AsNoTracking().ToList();
        

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return  query.ToList();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync() =>await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
           var query = _context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
           IQueryable<T> query = _context.Set<T>().Where(x=>x.Id==id);
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            
            return await (query).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int  id, T entity)
        {
            // Validate input parameters
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Retrieve the existing entity from the data source
            var existingEntity = await _context.Set<T>().FindAsync(id);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }

       
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

           
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                
                throw new InvalidOperationException("Could not update entity.", ex);
            }
        }
    }
}
