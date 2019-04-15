using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Models
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        [Required] public string Name { get; set; }

        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }

        public Tournament Tournament { get; set; }

        public int Matches { get; set; }

        public int Wins { get; set; }

        public int Loses { get; set; }

        public int Points { get; set; }
    }
}
