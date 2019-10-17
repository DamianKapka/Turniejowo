using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Turniejowo.API.Models
{
    public class Match
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MatchId { get; set; }

        [Required]
        public DateTime MatchDateTime { get; set; }

        [Required]
        public bool IsFinished { get; set; }

        [Required]
        public int? HomeTeamId { get; set; }

        public virtual Team HomeTeam { get; set; }

        [Required]
        public int? GuestTeamId { get; set; }

        public virtual Team GuestTeam { get; set; }

        [Required]
        public int HomeTeamPoints { get; set; }

        [Required]
        public int GuestTeamPoints { get; set; }

        public ICollection<Points> MatchPoints { get; set; }
    }
}
