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
using Turniejowo.API.Contracts.Requests;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Helpers;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.Services;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var user = await userService.GetUserByIdAsync(id);

                user.Password = null;

                return Ok(user);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch (Exception e)
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

                await userService.AddNewUserAsync(user);

                return CreatedAtAction("GetById", new {id = user.UserId}, user);
            }
            catch (AlreadyInDatabaseException)
            {
                return Conflict();
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
                var authenticatedUser = await userService.AuthenticateCredentialsAsync(credentials);

                var user = userService.AssignJwtToken(authenticatedUser);

                user.Password = null;

                return Ok(user);
            }
            catch (NotFoundInDatabaseException)
            {
                return Unauthorized();
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
                var tournaments = await userService.GetUserTournamentsAsync(id);

                return Ok(tournaments);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
