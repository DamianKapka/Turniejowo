using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface IPlayerService
    {
        Task<Player> GetPlayerById(int id);

        void AddNewPlayer(Player player);

        void EditPlayer(Player player);

        void DeletePlayer(int id);        
    }
}
