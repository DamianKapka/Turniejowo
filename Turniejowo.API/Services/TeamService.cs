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

        public async Task<Team> GetTeamByIdAsync(int id)
        {
            var team = await teamRepository.GetByIdAsync(id);

            if (team == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return team;
        }

        public async Task AddNewTeamAsync(Team team)
        {
            var tournamentForTeam = await tournamentRepository.GetByIdAsync(team.TournamentId);

            if (tournamentForTeam == null)
            {
                throw new NotFoundInDatabaseException();
            }

            var teamNameExistsForTournament =
                await teamRepository.FindSingleAsync(t => t.TournamentId == team.TournamentId && t.Name == team.Name);

            if (teamNameExistsForTournament != null)
            {
                throw new AlreadyInDatabaseException();
            }

            teamRepository.Add(team);
            await unitOfWork.CompleteAsync();
        }

        public async Task EditTeamAsync(Team team)
        {
            if (await tournamentRepository.GetByIdAsync(team.TournamentId) == null)
            {
                throw new NotFoundInDatabaseException();
            }

            var teamToEdit = await teamRepository.GetByIdAsync(team.TeamId);

            if (teamToEdit == null)
            {
                throw new NotFoundInDatabaseException();
            }

            teamRepository.ClearEntryState(teamToEdit);

            teamRepository.Update(team);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteTeamAsync(int id)
        {
            var teamToDelete = await teamRepository.GetByIdAsync(id);

            if (teamToDelete == null)
            {
                throw new NotFoundInDatabaseException();
            }

            teamRepository.Delete(teamToDelete);
            await unitOfWork.CompleteAsync();
        }

        public async Task<ICollection<Player>> GetTeamPlayersAsync(int id)
        {
            var players = await playerRepository.FindAsync(player => player.TeamId == id);

            return players;
        }

        public async Task<ICollection<Match>> GetTeamMatchesAsync(int id)
        {
            var matches = await matchRepository.FindAsync(m => m.HomeTeamId == id || m.GuestTeamId == id);

            return matches;
        }
    }
}