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
    public class TournamentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITeamRepository _teamRepository;

        public TournamentController(IUnitOfWork unitOfWork, ITournamentRepository tournamentRepository, ITeamRepository teamRepository)
        {
            _unitOfWork = unitOfWork;
            _tournamentRepository = tournamentRepository;
            _teamRepository = teamRepository;
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

                _tournamentRepository.Add(tournament);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction("GetById", new {id = tournament.TournamentId}, tournament);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var tournament = await _tournamentRepository.GetById(id);

                if (tournament == null)
                {
                    return NotFound("Tournament doesn't exist in database");
                }

                return Ok(tournament);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("{id}/teams")]
        public async Task<IActionResult> GetTeamsForTournament([FromRoute] int id)
        {
            try
            {
                var teams = await _teamRepository.Find(team => team.TournamentId == id);

                if (teams == null)
                {
                    return NotFound("Tournament doesn't exist in database");
                }

                return Ok(teams);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}