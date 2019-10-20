using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Contracts.Responses
{
    public class TournamentPlayersPointsHolder
    {
        public List<TournamentPlayerPoints> Content { get; set; }

        public TournamentPlayersPointsHolder(List<TournamentPlayerPoints> content)
        {
            Content = content;
        }
    }
}
