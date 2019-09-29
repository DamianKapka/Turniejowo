using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Contracts.Responses
{
    public class MatchResponse
    {
        public int MatchId { get; set; }

        public DateTime MatchDateTime { get; set; }

        public bool IsFinished { get; set; }

        public int HomeTeamId { get; set; }

        public string HomeTeamName { get; set; }

        public int GuestTeamId { get; set; }

        public string GuestTeamName { get; set; }

        public int HomeTeamPoints { get; set; }

        public int GuestTeamPoints { get; set; }
    }
}
