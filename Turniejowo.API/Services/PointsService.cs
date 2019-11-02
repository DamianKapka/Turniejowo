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
            var pointsList = points as List<Points> ?? throw new InvalidCastException();

            var matchPoints = await pointsRepository.FindAsync(p => p.MatchId == pointsList[0].MatchId, new string[] { "Player" });
            var match = await matchRepository.FindSingleAsync(m => m.MatchId == pointsList[0].MatchId) ?? throw new NotFoundInDatabaseException();
            var player = await playerRepository.FindSingleAsync(p => p.PlayerId == pointsList[0].PlayerId) ?? throw new NotFoundInDatabaseException();


            if ((player.TeamId != match.HomeTeamId) && (player.TeamId != match.GuestTeamId))
            {
                throw new ArgumentException();
            }

            if (matchPoints.SingleOrDefault(m => m.PlayerId == pointsList[0].PlayerId) != null)
            {
                throw new AlreadyInDatabaseException();
            }

            var teamsPoints = new
            {
                homeTeamPoints = matchPoints.Where(p => p.Player.TeamId == match.HomeTeamId).Sum(s => s.PointsQty),
                guestTeamPoints = matchPoints.Where(p => p.Player.TeamId == match.GuestTeamId).Sum(s => s.PointsQty)
            };

            if (((player.TeamId == match.HomeTeamId) && ((pointsList[0].PointsQty + teamsPoints.homeTeamPoints) > match.HomeTeamPoints)) 
                || ((player.TeamId == match.GuestTeamId) && ((pointsList[0].PointsQty + teamsPoints.guestTeamPoints) > match.GuestTeamPoints)))
            {
                throw new ArgumentOutOfRangeException();
            }

            foreach (var p in points)
            {
                pointsRepository.Add(p);
            }

            await unitOfWork.CompleteAsync();
        }

        [Obsolete]
        public async Task EditPointsForMatchAsync(ICollection<Points> points)
        {
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
    }
}
