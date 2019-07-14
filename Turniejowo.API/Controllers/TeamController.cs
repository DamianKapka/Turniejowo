using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.Services;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService teamService;

        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var teamToFind = await teamService.GetTeamById(id);

                return Ok(teamToFind);
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

        [HttpPost]
        public async Task<IActionResult> AddNewTeam([FromBody] Team team)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await teamService.AddNewTeam(team);

                return CreatedAtAction("GetById", new {id = team.TeamId}, team);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam([FromRoute] int id, [FromBody] Team team)
        {
            try
            {
                if (id != team.TeamId)
                {
                    return Conflict();
                }

                await teamService.EditTeam(team);

                return Accepted();
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam([FromRoute] int id)
        {
            try
            {
                await teamService.DeleteTeam(id);

                return Accepted();
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
        [HttpGet]
        [Route(@"{id}/players")]
        public async Task<IActionResult> GetPlayersForTeam([FromRoute] int id)
        {
            try
            {
                var players = await teamService.GetTeamPlayers(id);

                return Ok(players);
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
        [HttpGet]
        [Route(@"{id}/matches")]
        public async Task<IActionResult> GetMatchesForTeam([FromRoute]int id)
        {
            try
            {
                var matches = await teamService.GetTeamMatches(id);

                return Ok(matches);
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