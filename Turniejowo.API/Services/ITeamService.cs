using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface ITeamService
    {
        Task<Team> GetTeamById(int id);

        Task AddNewTeam(Team team);

        Task EditTeam(Team team);

        Task DeleteTeam(int id);

        Task<ICollection<Player>> GetTeamPlayers(int id);

        Task<ICollection<Match>> GetTeamMatches(int id);
    }
}
