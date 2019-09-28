using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turniejowo.API.Models;

namespace Turniejowo.API.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TurniejowoDbContext _context;

        public Repository(TurniejowoDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> query, string[] properties = null)
        {
            if (properties != null)
            {
                return await properties
                    .Aggregate(_context.Set<T>().Where(query), (current, property) => current.Include(property))
                    .ToListAsync();
            }

            return await _context.Set<T>().Where(query).ToListAsync();
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> query)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(query);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Add(T item)
        {
            _context.Set<T>().Add(item);
        }

        public void Update(T item)
        {
            _context.Update(item);
        }

        public void Delete(T item)
        {
            _context.Set<T>().Remove(item);
        }

        public void ClearEntryState(T obj)
        {
            _context.Entry(obj).State = EntityState.Detached;
        }
    }
}
