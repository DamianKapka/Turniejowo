using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;

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

        public void AddNewPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void DeletePlayer(int id)
        {
            throw new NotImplementedException();
        }

        public void EditPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public async Task<Player> GetPlayerById(int id)
        {
            var player = await playerRepository.GetById(id);

            return player;
        }
    }
}
