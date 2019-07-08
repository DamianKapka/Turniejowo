using Turniejowo.API.GenericRepository;
using Turniejowo.API.Models;

namespace Turniejowo.API.Repositories
{
    public class UserRepository : Repository<User>,IUserRepository
    {
        public UserRepository(TurniejowoDbContext context) : base(context)
        {
            
        }
    }
}
