using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Requests;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);

        Task AddNewUserAsync(User user);

        Task<User> AuthenticateCredentialsAsync(Credentials credentials);

        User AssignJwtToken(User user);

        Task<ICollection<Tournament>> GetUserTournamentsAsync(int id);
    }
}
