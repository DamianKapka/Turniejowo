using System.Collections.Generic;
using System.Threading.Tasks;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITournamentRepository tournamentRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IPlayerRepository playerRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IUnitOfWork unitOfWork;

        public TeamService(ITournamentRepository tournamentRepository, ITeamRepository teamRepository,
            IPlayerRepository playerRepository, IMatchRepository matchRepository, IUnitOfWork unitOfWork)
        {
            this.tournamentRepository = tournamentRepository;
            this.teamRepository = teamRepository;
            this.playerRepository = playerRepository;
            this.matchRepository = matchRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Team> GetTeamById(int id)
        {
            var team = await teamRepository.GetById(id);

            return team;
        }

        public void AddNewTeam(Team team)
        {
            throw new System.NotImplementedException();
        }

        public void EditTeam(Team team)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteTeam(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<Player>> GetTeamPlayers(int id)
        {
            var players = await playerRepository.Find(player => player.TeamId == id);
            return players;
        }

        public async Task<ICollection<Match>> GetTeamMatches(int id)
        {
            var matches = await matchRepository.Find(m => m.HomeTeamId == id || m.GuestTeamId == id);
            return matches;
        }
    }
}