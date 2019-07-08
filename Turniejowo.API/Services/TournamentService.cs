using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITournamentRepository tournamentRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IPlayerRepository playerRepository;

        public TournamentService(IUnitOfWork unitOfWork, ITournamentRepository tournamentRepository, ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            this.unitOfWork = unitOfWork;
            this.tournamentRepository = tournamentRepository;
            this.teamRepository = teamRepository;
            this.playerRepository = playerRepository;
        }

        public async Task<Tournament> GetTournamentById(int id)
        {
            var tournament = await tournamentRepository.GetById(id);
            return tournament;
        }

        public void AddNewTournament(Tournament tournament)
        {
            throw new System.NotImplementedException();
        }

        public void EditTournament(Tournament tournament)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteTournament(Tournament tournament)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<Team>> GetTournamentTeams(int id)
        {
            var teams = await teamRepository.Find(team => team.TournamentId == id);

            return teams;
        }

        public async Task<ICollection<Player>> GetTournamentPlayers(int id)
        {
            var teams = await teamRepository.Find(t => t.TournamentId == id);

            var players =
                await playerRepository.Find(p => teams.Select(t => t.TeamId).Contains(p.TeamId));

            return players;
        }

        public async Task<IDictionary<Team,List<Player>>> GetTournamentPlayersGroupedByTeam(int id)
        {
            var players = await GetTournamentPlayers(id);

            var playersGrouped = players.GroupBy(p => p.Team).ToDictionary(x => x.Key, y => y.ToList<Player>());

            return playersGrouped;
        }
    }
}