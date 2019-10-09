using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper mapper;

        public TournamentController(ITournamentService tournamentService, IMapper mapper)
        {
            this.tournamentService = tournamentService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var tournamentToFind = await tournamentService.GetTournamentByIdAsync(id);
                var tournamentTeams = await tournamentService.GetTournamentTeamsAsync(id);

                var response = mapper.Map<TournamentResponse>(tournamentToFind);
                response.AmountOfSignedTeams = tournamentTeams.Count;

                return Ok(response);
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

                return Ok(mapper.Map<List<TeamResponse>>(teams));
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
        public async Task<IActionResult> GetPlayersForTournament([FromRoute] int id, [FromQuery] bool groupedbyteam)
        {
            try
            {
                if (groupedbyteam)
                {
                    var groupedPlayers = await tournamentService.GetTournamentPlayersGroupedByTeamAsync(id);
                    return Ok(groupedPlayers);
                }

                var players = await tournamentService.GetTournamentPlayersAsync(id);
                return Ok(mapper.Map<List<PlayerResponse>>(players));
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
        [HttpGet("{id:int}/points")]
        public async Task<IActionResult> GetPointsForTournament([FromRoute] int tournamentId)
        {
            throw new NotImplementedException();
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
    }
}