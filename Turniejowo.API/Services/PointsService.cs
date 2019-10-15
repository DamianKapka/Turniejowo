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
        private readonly IMatchRepository matchRepository;
        private readonly IUnitOfWork unitOfWork;

        public PointsService(IPointsRepository pointsRepository, IMatchRepository matchRepository, IUnitOfWork unitOfWork)
        {
            this.pointsRepository = pointsRepository;
            this.matchRepository = matchRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task AddPointsForMatchAsync(ICollection<Points> points)
        {
            foreach (var p in points)
            {
                if (!ValidatePointsPropertiesRelations(p))
                {
                    throw new ArgumentException();
                }
            }

            foreach (var p in points)
            {
                pointsRepository.Add(p);
            }

            await unitOfWork.CompleteAsync();
        }

        public async Task EditPointsForMatchAsync(ICollection<Points> points)
        {
            foreach (var p in points)
            {
                if (!ValidatePointsPropertiesRelations(p))
                {
                    throw new ArgumentException();
                }
            }

            foreach (var p in points)
            {
                pointsRepository.Update(p);
            }

            await unitOfWork.CompleteAsync();
        }

        public async Task DeletePointsForMatchAsync(int matchId)
        {
            var match = await matchRepository.FindSingleAsync(m => m.MatchId == matchId) ?? throw new NotFoundInDatabaseException();

            var matchPoints = await pointsRepository.FindAsync(p => p.MatchId == matchId) ?? throw new NotFoundInDatabaseException();

            foreach (var m in matchPoints)
            {
                pointsRepository.Delete(m);
            }

            await unitOfWork.CompleteAsync();
;        }

        private bool ValidatePointsPropertiesRelations(Points points)
        {
            if (points.Player.TeamId != points.Match.HomeTeamId && points.Player.TeamId != points.Match.GuestTeamId)
            {
                return false;
            }

            return true;
        }
    }
}
