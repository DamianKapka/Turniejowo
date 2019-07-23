using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Contracts.Responses
{
    public class TeamWithPlayers
    {
        public Team Team;
        public Player[] Players;
    }
}
