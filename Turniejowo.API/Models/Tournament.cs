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

        [Required]
        public string Name { get; set; }

        [ForeignKey("Discipline")]
        public int DisciplineId { get; set; }

        [Required]
        public Discipline Discipline { get; set; }

        [ForeignKey("Creator")]
        public int CreatorId { get; set; }

        [Required]
        public User Creator { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int AmountOfTeams { get; set; }

        [Required]
        public int EntryFee { get; set; }

        [Required]
        public string Localization { get; set; }

    }
}
