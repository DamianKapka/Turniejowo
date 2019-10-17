using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface IPlayerService
    {
        Task<Player> GetPlayerByIdAsync(int id);

        Task<ICollection<Points>> GetPointsForPlayer(int playerId);

        Task AddNewPlayerAsync(Player player);

        Task EditPlayerAsync(Player player);

        Task DeletePlayerAsync(int id);        
    }
}
