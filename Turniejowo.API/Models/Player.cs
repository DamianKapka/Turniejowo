using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Models
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  PlayerId { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Insert First Name")]
        [RegularExpression("\\D+",ErrorMessage = "First Name cannot contain digits or special characters")]
        public string FName { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Insert Last Name")]
        [RegularExpression("\\D+",ErrorMessage = "Last Name cannot contain digits or special characters")]
        public string LName { get; set; }

        [ForeignKey("Team")] public int TeamId { get; set; }

        public Team Team { get; set; }

        public ICollection<Points> PlayerPoints { get; set; }
    }
}
