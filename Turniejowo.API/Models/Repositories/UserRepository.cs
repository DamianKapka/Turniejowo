using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models.GenericRepository;

namespace Turniejowo.API.Models.Repositories
{
    public class UserRepository : Repository<User>,IUserRepository
    {
        public UserRepository(TurniejowoDbContext context) : base(context)
        {
            
        }
    }
}
