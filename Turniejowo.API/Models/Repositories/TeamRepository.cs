using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models.GenericRepository;

namespace Turniejowo.API.Models.Repositories
{
    public class TeamRepository : Repository<Team>,ITeamRepository
    {
        public TeamRepository(TurniejowoDbContext context) : base(context)
        {
            
        }
    }
}
