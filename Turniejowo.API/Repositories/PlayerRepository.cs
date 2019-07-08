using Turniejowo.API.GenericRepository;
using Turniejowo.API.Models;

namespace Turniejowo.API.Repositories
{
    public class PlayerRepository : Repository<Player>,IPlayerRepository
    {
        public PlayerRepository(TurniejowoDbContext context) : base(context)
        {
            
        }
    }
}
