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
        private readonly IUnitOfWork unitOfWork;

        public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository, IUnitOfWork unitOfWork)
        {
            this.playerRepository = playerRepository;
            this.teamRepository = teamRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            var player = await playerRepository.GetByIdAsync(id);

            if (player == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return player;
        }

        public async Task AddNewPlayerAsync(Player player)
        {
            var teamForPlayer = await teamRepository.GetByIdAsync(player.TeamId);

            if (teamForPlayer == null)
            {
                throw new NotFoundInDatabaseException();
            }

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
            var playerTeam = await teamRepository.GetByIdAsync(player.TeamId);

            if (playerTeam == null)
            {
                throw new NotFoundInDatabaseException();
            }

            var playerToEdit = await playerRepository.GetByIdAsync(player.PlayerId);

            if (playerToEdit == null)
            {
                throw new NotFoundInDatabaseException();
            }

            playerRepository.ClearEntryState(playerToEdit);

            playerRepository.Update(player);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeletePlayerAsync(int id)
        {
            var playerToDelete = await playerRepository.GetByIdAsync(id);

            if (playerToDelete == null)
            {
                throw new NotFoundInDatabaseException();
            }

            playerRepository.Delete(playerToDelete);
            await unitOfWork.CompleteAsync();
        }
    }
}
