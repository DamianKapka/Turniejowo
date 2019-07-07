using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;

namespace Turniejowo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITournamentRepository tournamentRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IPlayerRepository playerRepository;

        public TournamentController(IUnitOfWork unitOfWork, ITournamentRepository tournamentRepository, ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            this.unitOfWork = unitOfWork;
            this.tournamentRepository = tournamentRepository;
            this.teamRepository = teamRepository;
            this.playerRepository = playerRepository;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var tournamentToFind = await tournamentRepository.GetById(id);
                
                if (tournamentToFind == null)
                {
                    return NotFound();
                }

                return Ok(tournamentToFind);
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

                tournamentRepository.Add(tournament);
                await unitOfWork.CompleteAsync();

                return CreatedAtAction("GetById", new {id = tournament.TournamentId}, tournament);
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                tournamentRepository.Update(tournament);
                await unitOfWork.CompleteAsync();

                return Accepted();
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
                var tournamentToDelete = await tournamentRepository.GetById(id);

                if (tournamentToDelete == null)
                {
                    return NotFound();
                }

                tournamentRepository.Delete(tournamentToDelete);
                await unitOfWork.CompleteAsync();

                return Accepted();
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
                if(await tournamentRepository.GetById(id) == null)
                {
                    NotFound();
                }

                var teams = await teamRepository.Find(team => team.TournamentId == id);

                if (teams == null)
                {
                    return NotFound();
                }

                return Ok(teams);
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
                if (await tournamentRepository.GetById(id) == null)
                {
                    return NotFound();
                }

                var teams = await teamRepository.Find(t => t.TournamentId == id);

                var players =
                    await playerRepository.Find(p => teams.Select(t => t.TeamId).Contains(p.TeamId));

                if (groupedbyteam)
                {
                    var teamsWithPlayers = new List<TeamWithPlayers>();
                    foreach (var team in teams)
                    {
                        var teamplayers = players.Where(p => p.TeamId == team.TeamId).ToArray();

                        teamsWithPlayers.Add(new TeamWithPlayers()
                        {
                            Team = team,
                            Players = teamplayers
                        });
                    }
                    return Ok(teamsWithPlayers);
                }

                return Ok(players);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}