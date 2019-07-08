using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface IMatchService
    {
        Task<ICollection<Match>> GetAllMatches();

        Task<Match> GetMatchById(int id);

        Task AddNewMatch(Match match);

        Task EditMatch(Match match);

        Task DeleteMatch(int id);
    }
}
