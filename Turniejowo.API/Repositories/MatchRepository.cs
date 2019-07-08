using Turniejowo.API.GenericRepository;
using Turniejowo.API.Models;

namespace Turniejowo.API.Repositories
{
    public class MatchRepository : Repository<Match>,IMatchRepository
    {
        public MatchRepository(TurniejowoDbContext context) : base(context)
        {

        }
    }
}
