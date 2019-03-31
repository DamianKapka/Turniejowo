using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Turniejowo.API.Models.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TurniejowoDbContext _context;

        public Repository(TurniejowoDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<ICollection<T>> Find(Expression<Func<T, bool>> query)
        {
            return await _context.Set<T>().Where(query).ToListAsync();
        }

        public async Task<T> FindSingle(Expression<Func<T, bool>> query)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(query);
        }

        public async Task<T> GetById(int id)
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
    }
}
