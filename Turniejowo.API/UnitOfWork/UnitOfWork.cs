using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TurniejowoDbContext context;

        public UnitOfWork(TurniejowoDbContext context)
        {
            this.context = context;
        }

        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
