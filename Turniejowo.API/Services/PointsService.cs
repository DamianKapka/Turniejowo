using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<ICollection<Points>> GetPointsForMatch(int matchId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Points>> GetPointsForTournament(int tournamentId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Points>> GetPointsForPlayer(int playerId)
        {
            throw new NotImplementedException();
        }

        public Task AddPointsForMatchAsync(ICollection<Points> points)
        {
            throw new NotImplementedException();
        }

        public Task EditPointsForMatchAsync(ICollection<Points> points)
        {
            throw new NotImplementedException();
        }

        public Task DeletePointsForMatchAsync(int matchId)
        {
            throw new NotImplementedException();
        }
    }
}
