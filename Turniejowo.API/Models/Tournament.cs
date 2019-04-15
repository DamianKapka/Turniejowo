using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Turniejowo.API.Models
{
    public class Tournament
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TournamentId { get; set; }

        [ForeignKey("Discipline")]
        public int DisciplineId { get; set; }

        public Discipline Discipline { get; set; }

        [ForeignKey("Creator")]
        public int CreatorId { get; set; }

        public User Creator { get; set; }

        public DateTime Date { get; set; }

        public int AmountOfTeams { get; set; }

        public int EntryFee { get; set; }

        public string Localization { get; set; }

    }
}
