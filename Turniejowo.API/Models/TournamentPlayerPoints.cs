using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Responses;

namespace Turniejowo.API.Models
{
    public class TournamentPlayerPoints
    {
        public int PlayerId { get; set; }
        public PlayerResponse Player{ get; set; }
        public int PointsQty { get; set; }
    }
}
