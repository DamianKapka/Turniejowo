using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turniejowo.API.GenericRepository;
using Turniejowo.API.Models;

namespace Turniejowo.API.Repositories
{
    public class PointsRepository : Repository<Points>, IPointsRepository
    {
        public PointsRepository(TurniejowoDbContext context) : base(context)
        {
            
        }
    }
}
