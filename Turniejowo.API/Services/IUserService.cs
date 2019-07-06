using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);

        void AddNewUser(User user);

        void AuthenticateUser(Credentials credentials);

        Task<ICollection<Tournament>> GetUserTournaments(int id);
    }
}
