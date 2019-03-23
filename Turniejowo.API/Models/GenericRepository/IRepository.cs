using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Turniejowo.API.Models.GenericRepository
{
    public interface IRepository<T>
    {
        Task<ICollection<T>> GetAll();
        Task<ICollection<T>> Find(Expression<Func<T, bool>> query);
        Task<T> FindSingle(Expression<Func<T, bool>> query);
        Task<T> GetById(int id);
        Task Add(T item);
        Task Update(T item);
        Task Delete(T item);
    }
}
