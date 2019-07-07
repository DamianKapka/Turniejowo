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
using Turniejowo.API.Helpers;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;

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

        public async Task<User> GetUserById(int id)
        {
            var user = await userRepository.GetById(id);
            return user;
        }

        public void AddNewUser(User user)
        {
            throw new NotImplementedException();
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

        public async Task<ICollection<Tournament>> GetUserTournaments(int id)
        {
            var tournaments = await tournamentRepository.Find(t => t.CreatorId == id);

            return tournaments;
        }
    }
}
