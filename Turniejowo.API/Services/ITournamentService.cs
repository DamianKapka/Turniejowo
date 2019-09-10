using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface ITournamentService
    {
        Task<Tournament> GetTournamentByIdAsync(int id);

        Task AddNewTournamentAsync(Tournament tournament);

        Task EditTournamentAsync(Tournament tournament);

        Task DeleteTournamentAsync(int id);

        Task<ICollection<Team>> GetTournamentTeamsAsync(int id);

        Task<ICollection<Player>> GetTournamentPlayersAsync(int id);

        Task<List<TeamWithPlayers>> GetTournamentPlayersGroupedByTeamAsync(int id);

        Task<ICollection<Match>> GetTournamentMatchesAsync(int id);

        Task<List<DateWithMatches>> GetTournamentMatchesGroupedByDateAsync(int id);

        Task<Table> GetTournamentTable(int id);
    }
}
