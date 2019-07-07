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
        Task<Tournament> GetTournamentById(int id);

        void AddNewTournament(Tournament tournament);

        void EditTournament(Tournament tournament);

        void DeleteTournament(Tournament tournament);

        Task<ICollection<Team>> GetTournamentTeams(int id);

        Task<ICollection<Player>> GetTournamentPlayers(int id);

        Task<IDictionary<Team, List<Player>>> GetTournamentPlayersGroupedByTeam(int id);
    }
}
