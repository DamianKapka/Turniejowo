using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Models
{
    public class BracketData
    {
        public int NumberOfTeams { get; set; }
        public List<BracketRound> Rounds { get; set; }
    }
}
