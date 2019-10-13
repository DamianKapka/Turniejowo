using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Contracts.Responses
{
    public class PointsResponse
    {
        public int PointsId { get; set; }
        public PlayerResponse Player { get; set; }
        public MatchResponse Match { get; set; }
        public TournamentResponse Tournament { get; set; }
        public int PointsQty { get; set; }
    }
}
