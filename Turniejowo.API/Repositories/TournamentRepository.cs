using Turniejowo.API.GenericRepository;
using Turniejowo.API.Models;

namespace Turniejowo.API.Repositories
{
    public class TournamentRepository : Repository<Tournament>,ITournamentRepository
    {
        public TournamentRepository(TurniejowoDbContext context) : base(context)
        {
            
        }
    }
}
