using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public Discipline Discipline { get; set; }

        [ForeignKey("Creator")]
        public int CreatorId { get; set; }

        public User Creator { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int AmountOfTeams { get; set; }

        [Required]
        public int EntryFee { get; set; }

        [Required]
        public string Localization { get; set; }

        [Required]
        public bool IsBracket { get; set; }

        public ICollection<Points> TournamentPoints { get; set; }
    }
}
