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
        private readonly IPlayerRepository playerRepository;
        private readonly IUnitOfWork unitOfWork;

        public PointsService(IPointsRepository pointsRepository, IMatchRepository matchRepository,IPlayerRepository playerRepository, IUnitOfWork unitOfWork)
        {
            this.pointsRepository = pointsRepository;
            this.matchRepository = matchRepository;
            this.playerRepository = playerRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task AddPointsForMatchAsync(ICollection<Points> points)
        {
            if (await ValidatePointsPropertiesRelations(points as List<Points>) == false)
            {
                throw new ArgumentException();
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
                if (await ValidatePointsPropertiesRelations(points as List<Points>) == false)
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

        private async Task<bool> ValidatePointsPropertiesRelations(List<Points> points)
        { 
            var match = await matchRepository.FindSingleAsync(m => m.MatchId == points[0].MatchId) ?? throw new NotFoundInDatabaseException();

            foreach (var p in points)
            {
                var player = await playerRepository.FindSingleAsync(pl => pl.PlayerId == p.PlayerId) ?? throw new NotFoundInDatabaseException();

                if ((player.TeamId != match.HomeTeamId) && (player.TeamId != match.GuestTeamId))
                {
                    return false;
                } 
            }

            return true;
        }
    }
}
