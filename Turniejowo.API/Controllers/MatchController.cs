using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.Services;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Controllers
{
    [Authorize]
    [Route("/api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService matchService;
        private readonly IMapper mapper;

        public MatchController(IMatchService matchService, IMapper mapper)
        {
            this.matchService = matchService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var matches = await matchService.GetAllMatchesAsync();
                return Ok(mapper.Map<List<MatchResponse>>(matches));
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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var match = await matchService.GetMatchByIdAsync(id);

                return Ok(mapper.Map<MatchResponse>(match));
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
        [HttpGet("{id:int}/points")]
        public async Task<IActionResult> GetPointsForMatch([FromRoute] int id)
        {
            try
            {
                var points = await matchService.GetPointsForMatch(id);

                return Ok(mapper.Map<List<PointsResponse>>(points));
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Match match)
        {
            try
            {
                if (match.HomeTeamId == match.GuestTeamId)
                {
                    return Conflict();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await matchService.AddNewMatchAsync(match);

                return CreatedAtAction("Get", new {id = match.MatchId}, match);
            }
            catch (ArgumentOutOfRangeException)
            {
                return StatusCode(427);
            }
            catch (ArgumentException)
            {
                return StatusCode(418);
            }
            catch (AlreadyInDatabaseException)
            {
                return StatusCode(406);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id,[FromBody] Match match)
        {
            try
            {
                if ((id != match.MatchId) || (match.HomeTeamId == match.GuestTeamId))
                {
                    return Conflict();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await matchService.EditMatchAsync(match);

                return Accepted();
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await matchService.DeleteMatchAsync(id);

                return Accepted();
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    } 
}
