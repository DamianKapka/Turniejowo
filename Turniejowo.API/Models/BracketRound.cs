using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Responses;

namespace Turniejowo.API.Models
{
    public class BracketRound
    {
        public int RoundIndex { get; set; }
        public List<MatchResponse> Matches { get; set; }
    }
}
