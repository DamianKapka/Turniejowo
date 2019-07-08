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
        private readonly IUnitOfWork unitOfWork;
        private readonly ITeamRepository teamRepository;
        private readonly IPlayerRepository playerRepository;

        public PlayerService(IUnitOfWork unitOfWork, ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            this.unitOfWork = unitOfWork;
            this.teamRepository = teamRepository;
            this.playerRepository = playerRepository;
        }

        public async Task AddNewPlayer(Player player)
        {
            var teamForPlayer = await teamRepository.GetById(player.TeamId);

            if (teamForPlayer == null)
            {
                throw new NotFoundInDatabaseException();
            }

            var playerNameExistsForTeam =
                await playerRepository.FindSingle(p => p.TeamId == player.TeamId && p.FName == player.FName && p.LName == player.LName);

            if (playerNameExistsForTeam != null)
            {
                throw new AlreadyInDatabaseException();
            }

            playerRepository.Add(player);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeletePlayer(int id)
        {
            var playerToDelete = await playerRepository.GetById(id);

            if (playerToDelete == null)
            {
                throw new NotFoundInDatabaseException();
            }

            playerRepository.Delete(playerToDelete);
            await unitOfWork.CompleteAsync();
        }

        public async Task EditPlayer(Player player)
        {
            var playerToEdit = await playerRepository.GetById(player.PlayerId);

            if (playerToEdit== null)
            {
                throw new NotFoundInDatabaseException();
            }

            playerRepository.ClearEntryState(playerToEdit);

            playerRepository.Update(player);
            await unitOfWork.CompleteAsync();
        }

        public async Task<Player> GetPlayerById(int id)
        {
            var player = await playerRepository.GetById(id);

            if (player == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return player;
        }
    }
}
