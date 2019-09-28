using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Turniejowo.API.GenericRepository
{
    public interface IRepository<T>
    {
        Task<ICollection<T>> GetAllAsync(string[] properties = null);
        Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> query, string[] properties = null);
        Task<T> FindSingleAsync(Expression<Func<T, bool>> query, string[] properties = null);
        Task<T> GetByIdAsync(int id);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        void ClearEntryState(T obj);
    }
}
