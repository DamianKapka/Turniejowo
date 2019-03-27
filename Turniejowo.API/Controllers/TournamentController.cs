using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
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
                var tournamentToFind = await _tournamentRepository.GetById(id);

                if (tournamentToFind == null)
                {
                    return NotFound("Tournament doesn't exist in database");
                }

                return Ok(tournamentToFind);
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

                var tournamentToUpdate = await _tournamentRepository.GetById(id);

                if (tournamentToUpdate == null)
                {
                    return NotFound("Tournament doesn't exist in database");
                }

                _tournamentRepository.Update(tournament);
                await _unitOfWork.CompleteAsync();

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
                var tournamentToDelete = await _tournamentRepository.GetById(id);

                if (tournamentToDelete == null)
                {
                    return NotFound("Tournament doesn't exist in database");
                }

                _tournamentRepository.Delete(tournamentToDelete);
                await _unitOfWork.CompleteAsync();

                return Accepted();
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