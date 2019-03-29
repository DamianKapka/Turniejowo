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
    public class PlayerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public PlayerController(IUnitOfWork unitOfWork,ITeamRepository teamRepository,IPlayerRepository playerRepository)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var playerToFind = await _playerRepository.GetById(id);

                if (playerToFind == null)
                {
                    return NotFound("Player doesn't exist in database");
                }

                return Ok(playerToFind);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPlayer([FromBody] Player player)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var teamForPlayer = await _teamRepository.GetById(player.TeamId);

                if (teamForPlayer == null)
                {
                    return NotFound("Such team doesn't exist");
                }

                var playerNameExistsForTeam =
                    await _playerRepository.FindSingle(p => p.TeamId == player.TeamId && p.FName == player.FName && p.LName == player.LName);

                if (playerNameExistsForTeam != null)
                {
                    return BadRequest("Player name for the team already exists");
                }

                _playerRepository.Add(player);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction("GetById", new { id = player.TeamId }, player);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer([FromRoute] int id)
        {
            try
            {
                var playerToDelete = await _playerRepository.GetById(id);

                if (playerToDelete == null)
                {
                    return NotFound("Player doesn't exist in database");
                }

                _playerRepository.Delete(playerToDelete);
                await _unitOfWork.CompleteAsync();

                return Accepted();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer([FromRoute] int id, [FromBody] Player player)
        {
            try
            {
                if (id != player.TeamId)
                {
                    return BadRequest("Id of edited player and updated one don't match");
                }

                if (await _playerRepository.GetById(id) == null)
                {
                    return BadRequest("Original player not found in database");
                }

                _playerRepository.Update(player);
                await _unitOfWork.CompleteAsync();

                return Accepted();
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}");
            }
        }
    }
}