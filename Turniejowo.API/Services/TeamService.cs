using System.Collections.Generic;
using System.Threading.Tasks;
using Turniejowo.API.Exceptions;
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

            if (team == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return team;
        }

        public async Task AddNewTeam(Team team)
        {
            var tournamentForTeam = await tournamentRepository.GetById(team.TournamentId);

            if (tournamentForTeam == null)
            {
                throw new NotFoundInDatabaseException();
            }

            var teamNameExistsForTournament =
                await teamRepository.FindSingle(t => t.TournamentId == team.TournamentId && t.Name == team.Name);

            if (teamNameExistsForTournament != null)
            {
                throw new AlreadyInDatabaseException();
            }

            teamRepository.Add(team);
            await unitOfWork.CompleteAsync();
        }

        public async Task EditTeam(Team team)
        {
            var teamToEdit = await teamRepository.GetById(team.TeamId);

            if (teamToEdit == null)
            {
                throw new NotFoundInDatabaseException();
            }

            teamRepository.ClearEntryState(teamToEdit);

            teamRepository.Update(team);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteTeam(int id)
        {
            var teamToDelete = await teamRepository.GetById(id);

            if (teamToDelete == null)
            {
                throw new NotFoundInDatabaseException();
            }

            teamRepository.Delete(teamToDelete);
            await unitOfWork.CompleteAsync();
        }

        public async Task<ICollection<Player>> GetTeamPlayers(int id)
        {
            var players = await playerRepository.Find(player => player.TeamId == id);

            if (players.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return players;
        }

        public async Task<ICollection<Match>> GetTeamMatches(int id)
        {
            var matches = await matchRepository.Find(m => m.HomeTeamId == id || m.GuestTeamId == id);

            if (matches.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return matches;
        }
    }
}