using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Models
{
    public class Match
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MatchId { get; set; }

        public int? HomeTeamId { get; set; }

        public virtual Team HomeTeam { get; set; }

        public int? GuestTeamId { get; set; }

        public virtual Team GuestTeam { get; set; }

        public int HomeTeamPoints { get; set; }

        public int GuestTeamPoints { get; set; }
    }
}
