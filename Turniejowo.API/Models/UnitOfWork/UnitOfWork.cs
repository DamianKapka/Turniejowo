using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Models.UnitOfWork
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
