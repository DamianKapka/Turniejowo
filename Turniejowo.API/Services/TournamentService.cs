using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Exceptions;
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
        private readonly IUserRepository userRepository;

        public TournamentService(IUnitOfWork unitOfWork, ITournamentRepository tournamentRepository, ITeamRepository teamRepository, IPlayerRepository playerRepository, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.tournamentRepository = tournamentRepository;
            this.teamRepository = teamRepository;
            this.playerRepository = playerRepository;
            this.userRepository = userRepository;
        }

        public async Task<Tournament> GetTournamentById(int id)
        {
            var tournament = await tournamentRepository.GetById(id);

            if (tournament == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return tournament;
        }

        public async Task AddNewTournament(Tournament tournament)
        {
            if (await userRepository.GetById(tournament.CreatorId) == null)
            {
                throw new NotFoundInDatabaseException();
            }

            var tournamentToAdd = await tournamentRepository.FindSingle(x => x.Name == tournament.Name);

            if (tournamentToAdd != null)
            {
                throw new AlreadyInDatabaseException();
            }

            tournamentRepository.Add(tournament);
            await unitOfWork.CompleteAsync();
        }

        public async Task EditTournament(Tournament tournament)
        {
            if (await userRepository.GetById(tournament.CreatorId) == null)
            {
                throw new NotFoundInDatabaseException();
            }

            var tournamentToEdit = await tournamentRepository.FindSingle(x => x.TournamentId == tournament.TournamentId);

            if (tournamentToEdit == null)
            {
                throw new NotFoundInDatabaseException();
            }

            tournamentRepository.ClearEntryState(tournamentToEdit);

            tournamentRepository.Update(tournament);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteTournament(int id)
        {
            var tournamentToDelete = await tournamentRepository.GetById(id);

            if (tournamentToDelete == null)
            {
                throw new NotFoundInDatabaseException();
            }

            tournamentRepository.Delete(tournamentToDelete);
            await unitOfWork.CompleteAsync();
        }

        public async Task<ICollection<Team>> GetTournamentTeams(int id)
        {
            var teams = await teamRepository.Find(team => team.TournamentId == id);

            if (teams.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return teams;
        }

        public async Task<ICollection<Player>> GetTournamentPlayers(int id)
        {
            var teams = await teamRepository.Find(t => t.TournamentId == id);

            if (teams.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            var players =
                await playerRepository.Find(p => teams.Select(t => t.TeamId).Contains(p.TeamId));

            if (players.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return players;
        }

        public async Task<IDictionary<Team,List<Player>>> GetTournamentPlayersGroupedByTeam(int id)
        {
            var players = await GetTournamentPlayers(id);

            if (players.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            var playersGrouped = players.GroupBy(p => p.Team).ToDictionary(x => x.Key, y => y.ToList<Player>());

            return playersGrouped;
        }
    }
}