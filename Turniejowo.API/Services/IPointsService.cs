using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface IPointsService
    {
        Task<ICollection<Points>> GetPointsForMatch(int matchId);
        Task<ICollection<Points>> GetPointsForTournament(int tournamentId);
        Task<ICollection<Points>> GetPointsForPlayer(int playerId);
        Task AddPointsForMatchAsync(ICollection<Points> points);
        Task EditPointsForMatchAsync(ICollection<Points> points);
        Task DeletePointsForMatchAsync(int matchId);
    }
}
