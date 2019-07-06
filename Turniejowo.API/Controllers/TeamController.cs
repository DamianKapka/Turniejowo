using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;

namespace Turniejowo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITournamentRepository tournamentRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IPlayerRepository playerRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IUnitOfWork unitOfWork;

        public TeamController(ITournamentRepository tournamentRepository,ITeamRepository teamRepository,IPlayerRepository playerRepository,IUnitOfWork unitOfWork,IMatchRepository matchRepository)
        {
            this.tournamentRepository = tournamentRepository;
            this.teamRepository = teamRepository;
            this.playerRepository = playerRepository;
            this.matchRepository = matchRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var teamToFind = await teamRepository.GetById(id);

                if (teamToFind == null)
                {
                    return NotFound();
                }

                return Ok(teamToFind);
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

                var tournamentForTeam = await tournamentRepository.GetById(team.TournamentId);

                if (tournamentForTeam == null)
                {
                    return NotFound();
                }

                var teamNameExistsForTournament =
                    await teamRepository.FindSingle(t => t.TournamentId == team.TournamentId && t.Name == team.Name);

                if (teamNameExistsForTournament != null)
                {
                    return Conflict();
                }

                teamRepository.Add(team);
                await unitOfWork.CompleteAsync();

                return CreatedAtAction("GetById", new {id = team.TeamId}, team);
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
                var teamToDelete = await teamRepository.GetById(id);

                if (teamToDelete == null)
                {
                    return NotFound();
                }

                teamRepository.Delete(teamToDelete);
                await unitOfWork.CompleteAsync();

                return Accepted();
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

                teamRepository.Update(team);
                await unitOfWork.CompleteAsync();

                return Accepted();
            }

            catch (Exception e)
            {
               return  BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route(@"{id}/players")]
        public async Task<IActionResult> GetPlayersForTeam([FromRoute] int id)
        {
            try
            {
                if (await teamRepository.GetById(id) == null)
                {
                    return NotFound();
                }

                var players = await playerRepository.Find(player => player.TeamId == id);

                if (players == null)
                {
                    return NotFound();
                }

                return Ok(players);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route(@"{id}/matches")]
        public async Task<IActionResult> GetMatchesForTeam([FromRoute]int id)
        {
            try
            {
                var matches = await matchRepository.Find(m => m.HomeTeamId == id || m.GuestTeamId == id);

                if (matches == null)
                {
                    return NotFound("No matches for that team");
                }

                return Ok(matches);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}