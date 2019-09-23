using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface IMatchService
    {
        Task<ICollection<Match>> GetAllMatchesAsync();

        Task<Match> GetMatchByIdAsync(int id);

        Task AddNewMatchAsync(Match match);

        Task EditMatchAsync(Match match);

        Task DeleteMatchAsync(int id);

        Task DeleteMatchesRelatedToTheTeamAsync(int id);
    }
}
