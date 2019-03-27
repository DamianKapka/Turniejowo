using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;

namespace Turniejowo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TeamController(ITournamentRepository tournamentRepository,ITeamRepository teamRepository,IUnitOfWork unitOfWork)
        {
            _tournamentRepository = tournamentRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById([FromRoute] int id)
        {
            try
            {
                var teamToFind = await _teamRepository.GetById(id);

                if (teamToFind == null)
                {
                    return NotFound("Team doesn't exist in database");
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

                var tournamentForTeam = await _tournamentRepository.GetById(team.TournamentId);

                if (tournamentForTeam == null)
                {
                    return NotFound("Such tournament doesn't exist");
                }

                var teamNameExistsForTournament =
                    await _teamRepository.FindSingle(t => t.TournamentId == team.TournamentId && t.Name == team.Name);

                if (teamNameExistsForTournament != null)
                {
                    return BadRequest("Team Name for the tournament already exists");
                }

                _teamRepository.Add(team);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction("GetTeamById", new {id = team.TeamId}, team);
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
                var teamToDelete = await _teamRepository.GetById(id);

                if (teamToDelete == null)
                {
                    return NotFound("Team doesn't exist in database");
                }

                _teamRepository.Delete(teamToDelete);
                await _unitOfWork.CompleteAsync();

                return Accepted();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}