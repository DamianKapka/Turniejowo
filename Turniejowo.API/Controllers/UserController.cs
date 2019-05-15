using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Turniejowo.API.Helpers;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;

namespace Turniejowo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ITournamentRepository tournamentRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly AppSettings appSettings;

        public UserController(IUserRepository userRepository,ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork,IOptions<AppSettings> appSettings)
        {
            this.userRepository = userRepository;
            this.tournamentRepository = tournamentRepository;
            this.unitOfWork = unitOfWork;
            this.appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var user = await userRepository.FindSingle(x => x.UserId == id);

                if (user == null)
                {
                    return NotFound("User does no exist");
                }

                user.Password = null;

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/tournaments")]
        public async Task<IActionResult> GetUserTournaments([FromRoute]int id)
        {
            try
            {
                var tournaments = await tournamentRepository.Find(t => t.CreatorId == id);

                if(tournamentRepository == null || tournaments.Count == 0)
                {
                    return NoContent();
                }

                return Ok(tournaments);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await userRepository.FindSingle(x => x.Email == user.Email) != null)
                {
                    return Conflict("Account with that e-mail already exists in database.");
                }

                userRepository.Add(user);
                await unitOfWork.CompleteAsync();

                return CreatedAtAction("GetById", new {id = user.UserId}, user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("{authenticate}")]
        public async Task<IActionResult> Authenticate([FromBody]Credentials credentials)
        {
            try
            {
                var user = await userRepository.FindSingle(x =>
                    x.Email == credentials.Login && x.Password == credentials.Password);

                if (user == null)
                {
                    return Unauthorized("Credentials invalid");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new[]
                        {
                            new Claim(ClaimTypes.Name, user.UserId.ToString()), 
                        }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);

                user.Password = null;

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
