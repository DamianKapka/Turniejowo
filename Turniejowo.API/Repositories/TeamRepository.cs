using Turniejowo.API.GenericRepository;
using Turniejowo.API.Models;

namespace Turniejowo.API.Repositories
{
    public class TeamRepository : Repository<Team>,ITeamRepository
    {
        public TeamRepository(TurniejowoDbContext context) : base(context)
        {
            
        }
    }
}
