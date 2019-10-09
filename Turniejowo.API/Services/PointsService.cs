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
        private readonly ITournamentRepository tournamentRepository;
        private readonly IPlayerRepository playerRepository;
        private readonly IUnitOfWork unitOfWork;

        public PointsService(PointsRepository pointsRepository,IUnitOfWork unitOfWork, IMatchRepository matchRepository, ITournamentRepository tournamentRepository, IPlayerRepository playerRepository)
        {
            this.pointsRepository = pointsRepository;
            this.unitOfWork = unitOfWork;
            this.matchRepository = matchRepository;
            this.tournamentRepository = tournamentRepository;
            this.playerRepository = playerRepository;
        }

        public async Task<ICollection<Points>> GetPointsForMatch(int matchId)
        {
            var match = await matchRepository.FindSingleAsync(m => m.MatchId == matchId) 
                        ?? throw new NotFoundInDatabaseException();

            var points = await pointsRepository.FindAsync(p => p.MatchId == matchId, new string[] { "Tournament", "Player", "Match" });

            return points;
        }

        public async Task<ICollection<Points>> GetPointsForTournament(int tournamentId)
        {
            var tournament = await tournamentRepository.FindSingleAsync(t => t.TournamentId == tournamentId) 
                             ?? throw new NotFoundInDatabaseException();

            var points = await pointsRepository.FindAsync(p => p.TournamentId == tournamentId,new string[]{"Tournament","Player","Match"});

            return points;
        }

        public async Task<ICollection<Points>> GetPointsForPlayer(int playerId)
        {
            var player = await playerRepository.FindSingleAsync(p => p.PlayerId == playerId)
                         ?? throw new NotFoundInDatabaseException();

            var points = await pointsRepository.FindAsync(p => p.PlayerId == playerId, new string[] { "Tournament", "Player", "Match" });

            return points;
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
