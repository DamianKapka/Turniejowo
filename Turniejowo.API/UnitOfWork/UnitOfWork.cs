using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TurniejowoDbContext _context;

        public UnitOfWork(TurniejowoDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
