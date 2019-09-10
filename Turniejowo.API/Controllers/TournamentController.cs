using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turniejowo.API.Contracts.Responses;
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
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            this.tournamentService = tournamentService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var tournamentToFind = await tournamentService.GetTournamentByIdAsync(id);

                return Ok(tournamentToFind);
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
        public async Task<IActionResult> AddNewTournament([FromBody] Tournament tournament)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await tournamentService.AddNewTournamentAsync(tournament);

                return CreatedAtAction("GetById", new {id = tournament.TournamentId}, tournament);
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
        public async Task<IActionResult> UpdateTournament([FromRoute] int id, [FromBody] Tournament tournament)
        {
            try
            {
                if (id != tournament.TournamentId)
                {
                    return Conflict();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await tournamentService.EditTournamentAsync(tournament);

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
        public async Task<IActionResult> DeleteTournament([FromRoute] int id)
        {
            try
            {
                await tournamentService.DeleteTournamentAsync(id);

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
     
        [HttpGet]
        [AllowAnonymous]
        [Route("{id}/teams")]
        public async Task<IActionResult> GetTeamsForTournament([FromRoute] int id)
        {
            try
            {
                var teams = await tournamentService.GetTournamentTeamsAsync(id);

                return Ok(teams);
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
        [Route("{id}/players")]
        public async Task<IActionResult> GetPlayersForTournament([FromRoute] int id,[FromQuery] bool groupedbyteam)
        {
            try
            {
                if (groupedbyteam)
                {
                    var groupedPlayers = await tournamentService.GetTournamentPlayersGroupedByTeamAsync(id);
                    return Ok(groupedPlayers);
                }

                var players = await tournamentService.GetTournamentPlayersAsync(id);
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
        [Route("{id}/matches")]
        public async Task<IActionResult> GetMatchesForTournament([FromRoute] int id, [FromQuery]bool groupedByDateTime)
        {
            try
            {
                if (groupedByDateTime)
                {
                    var groupedMatches = await tournamentService.GetTournamentMatchesGroupedByDateAsync(id);
                    return Ok(groupedMatches);
                }

                var matches = await tournamentService.GetTournamentMatchesAsync(id);

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

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}/table")]
        public async Task<IActionResult> GetTournamentTable([FromRoute] int id)
        {
            try
            {
                var table = await tournamentService.GetTournamentTable(id);

                return Ok(table);

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