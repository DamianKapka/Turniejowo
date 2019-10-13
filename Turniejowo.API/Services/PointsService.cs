using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Services
{
    public class PointsService : IPointsService
    {
        private readonly IPointsRepository pointsRepository;
        private readonly IUnitOfWork unitOfWork;

        public PointsService(PointsRepository pointsRepository,IUnitOfWork unitOfWork)
        {
            this.pointsRepository = pointsRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task AddPointsForMatchAsync(ICollection<Points> points)
        {
            throw new NotImplementedException();
        }

        public async Task EditPointsForMatchAsync(ICollection<Points> points)
        {
            throw new NotImplementedException();
        }

        public async Task DeletePointsForMatchAsync(int matchId)
        {
            throw new NotImplementedException();
        }
    }
}
