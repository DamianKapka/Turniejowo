using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Models
{
    public class GeneratorScheduleOutlines
    {
        public int TournamentId { get; set; }
        public ICollection<int> DaysOfWeek { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int SimultaneousMatches { get; set; }
        public bool Rematch { get; set; }
    }
}
