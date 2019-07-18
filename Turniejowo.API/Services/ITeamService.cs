using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface ITeamService
    {
        Task<Team> GetTeamByIdAsync(int id);

        Task AddNewTeamAsync(Team team);

        Task EditTeamAsync(Team team);

        Task DeleteTeamAsync(int id);

        Task<ICollection<Player>> GetTeamPlayersAsync(int id);

        Task<ICollection<Match>> GetTeamMatchesAsync(int id);
    }
}
