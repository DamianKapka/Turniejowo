using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Contracts.Responses
{
    public class TournamentResponse
    {
        public int TournamentId { get; set; }

        public string Name { get; set; }

        public int DisciplineId { get; set; }

        public string Discipline { get; set; }

        public string CreatorName { get; set; }

        public string CreatorContact { get; set; }

        public DateTime Date { get; set; }

        public int AmountOfTeams { get; set; }

        public int AmountOfSignedTeams { get; set; }

        public int EntryFee { get; set; }

        public string Localization { get; set; }

        public bool IsBracket { get; set; }
    }
}
