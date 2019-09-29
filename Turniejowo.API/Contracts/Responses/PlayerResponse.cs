using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Contracts.Responses
{
    public class PlayerResponse
    {
        public int PlayerId { get; set; }

        public string FName { get; set; }

        public string LName { get; set; }

        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public int Points { get; set; }
    }
}
