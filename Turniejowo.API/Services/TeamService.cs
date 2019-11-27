using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly IMatchService matchService;
        private readonly IUnitOfWork unitOfWork;

        public TeamService(ITournamentRepository tournamentRepository, ITeamRepository teamRepository,
            IPlayerRepository playerRepository, IMatchRepository matchRepository, IUnitOfWork unitOfWork, IMatchService matchService)
        {
            this.tournamentRepository = tournamentRepository;
            this.teamRepository = teamRepository;
            this.playerRepository = playerRepository;
            this.matchRepository = matchRepository;
            this.unitOfWork = unitOfWork;
            this.matchService = matchService;
        }

        public async Task<Team> GetTeamByIdAsync(int id)
        {
            var team = await teamRepository.GetByIdAsync(id) ?? throw new NotFoundInDatabaseException(); ;

            return team;
        }

        public async Task<ICollection<Player>> GetTeamPlayersAsync(int id)
        {
            var team = await teamRepository.GetByIdAsync(id) ?? throw new NotFoundInDatabaseException();

            var players = await playerRepository.FindAsync(player => player.TeamId == id,new string[]{"Team"});

            return players;
        }

        public async Task<ICollection<Match>> GetTeamMatchesAsync(int id)
        {
            var team = await teamRepository.GetByIdAsync(id) ?? throw new NotFoundInDatabaseException();

            var matches = await matchRepository.FindAsync(m => m.HomeTeamId == id || m.GuestTeamId == id,new string[]{"HomeTeam","GuestTeam"});

            return matches;
        }

        public async Task AddNewTeamAsync(Team team)
        {
            var tournamentForTeam = await tournamentRepository.GetByIdAsync(team.TournamentId) ?? throw new NotFoundInDatabaseException(); ;

            var tournamentTeams = await teamRepository.FindAsync(t => t.TournamentId == team.TournamentId);

            if (tournamentTeams.Count == tournamentForTeam.AmountOfTeams)
            {
                throw new ApplicationException();
                //TODO: TESTY DO TEGO!
            }

            if (tournamentTeams.First(t => t.Name == team.Name) != null)
            {
                throw new AlreadyInDatabaseException();
            }

            teamRepository.Add(team);
            await unitOfWork.CompleteAsync();
        }

        public async Task EditTeamAsync(Team team)
        {
            var tournament = await tournamentRepository.GetByIdAsync(team.TournamentId) ?? throw new NotFoundInDatabaseException();

            var teamToEdit = await teamRepository.GetByIdAsync(team.TeamId) ?? throw new NotFoundInDatabaseException(); ;

            teamRepository.ClearEntryState(teamToEdit);

            teamRepository.Update(team);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteTeamAsync(int id)
        {
            await matchService.DeleteMatchesRelatedToTheTeamAsync(id);
            var teamToDelete = await teamRepository.GetByIdAsync(id) ?? throw new NotFoundInDatabaseException(); ;

            teamRepository.Delete(teamToDelete);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteTeamsRelatedToTheTournamentAsync(int id)
        {
            var teams = await teamRepository.FindAsync(t => t.TournamentId == id);

            foreach (var team in teams)
            {
                await DeleteTeamAsync(team.TeamId);
            }

            await unitOfWork.CompleteAsync();
        }
    }
}