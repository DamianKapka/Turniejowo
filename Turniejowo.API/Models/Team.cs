using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        [Required]
        public int Matches { get; set; }

        [Required]
        public int Wins { get; set; }

        [Required]
        public int Draws { get; set; }

        [Required]
        public int Loses { get; set; }

        [Required]
        public int Points { get; set; }

        [JsonIgnore]
        public virtual ICollection<Match> HomeMatches{ get; set; }
        [JsonIgnore]
        public virtual ICollection<Match> GuestMatches{ get; set; }
    }
}
