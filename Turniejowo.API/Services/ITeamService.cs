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

        void AddNewTeam(Team team);

        void EditTeam(Team team);

        void DeleteTeam(int id);

        Task<ICollection<Player>> GetTeamPlayers(int id);

        Task<ICollection<Match>> GetTeamMatches(int id);
    }
}
