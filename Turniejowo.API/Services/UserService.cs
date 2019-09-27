using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Turniejowo.API.Contracts.Requests;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Helpers;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ITournamentRepository tournamentRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly AppSettings appSettings;

        public UserService(IUserRepository userRepository, ITournamentRepository tournamentRepository,
            IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            this.userRepository = userRepository;
            this.tournamentRepository = tournamentRepository;
            this.unitOfWork = unitOfWork;
            this.appSettings = appSettings.Value;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return user;
        }

        public async Task<ICollection<Tournament>> GetUserTournamentsAsync(int id)
        {
            var tournaments = await tournamentRepository.FindAsync(t => t.CreatorId == id);

            if (tournaments.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return tournaments;
        }

        public async Task AddNewUserAsync(User user)
        {
            if (await userRepository.FindSingleAsync(x => x.Email == user.Email) != null)
            {
                throw new AlreadyInDatabaseException();
            }

            userRepository.Add(user);
            await unitOfWork.CompleteAsync();
        }

        public async Task<User> AuthenticateCredentialsAsync(Credentials credentials)
        {
            var user = await userRepository.FindSingleAsync(
                u => u.Email == credentials.Login && u.Password == credentials.Password);

            if (user == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return user;
        }

        public User AssignJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, user.UserId.ToString()),
                        new Claim(ClaimTypes.Actor, user.FullName),
                    }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;

            return user;
        }
    }
}
