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
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository playerRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IPointsRepository pointsRepository;
        private readonly IUnitOfWork unitOfWork;
        
        public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository, IPointsRepository pointsRepository, IUnitOfWork unitOfWork)
        {
            this.playerRepository = playerRepository;
            this.teamRepository = teamRepository;
            this.pointsRepository = pointsRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            var player = await playerRepository.FindSingleAsync(p => p.PlayerId == id,new string[]{"Team"}) ?? throw new NotFoundInDatabaseException();

            return player;
        }

        public async Task<ICollection<Points>> GetPointsForPlayer(int playerId)
        {
            var player = await playerRepository.FindSingleAsync(p => p.PlayerId == playerId)
                         ?? throw new NotFoundInDatabaseException();

            var points = await pointsRepository.FindAsync(p => p.PlayerId == playerId, new string[] { "Tournament", "Player", "Match" });

            return points;
        }

        public async Task AddNewPlayerAsync(Player player)
        {
            var teamForPlayer = await teamRepository.GetByIdAsync(player.TeamId) ?? throw new NotFoundInDatabaseException(); ;

            var playerNameExistsForTeam =
                await playerRepository.FindSingleAsync(p => p.TeamId == player.TeamId && p.FName == player.FName && p.LName == player.LName);

            if (playerNameExistsForTeam != null)
            {
                throw new AlreadyInDatabaseException();
            }

            playerRepository.Add(player);
            await unitOfWork.CompleteAsync();
        }

        public async Task EditPlayerAsync(Player player)
        {
            var playerTeam = await teamRepository.GetByIdAsync(player.TeamId) ?? throw new NotFoundInDatabaseException(); ;

            var playerToEdit = await playerRepository.GetByIdAsync(player.PlayerId) ?? throw new NotFoundInDatabaseException(); ;

            playerRepository.ClearEntryState(playerToEdit);

            playerRepository.Update(player);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeletePlayerAsync(int id)
        {
            var playerToDelete = await playerRepository.GetByIdAsync(id) ?? throw new NotFoundInDatabaseException(); ;

            playerRepository.Delete(playerToDelete);
            await unitOfWork.CompleteAsync();
        }
    }
}
