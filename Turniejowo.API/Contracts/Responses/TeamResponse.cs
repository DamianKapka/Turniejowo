using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Contracts.Responses
{
    public class TeamResponse
    {
        public int TeamId { get; set; }

        public string Name { get; set; }

        public int TournamentId { get; set; }

        public int Matches { get; set; }

        public int Wins { get; set; }

        public int Draws { get; set; }

        public int Loses { get; set; }

        public int Points { get; set; }
    }
}
