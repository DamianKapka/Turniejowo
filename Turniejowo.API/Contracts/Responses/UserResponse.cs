using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Contracts.Responses
{
    public class UserResponse
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Token { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
