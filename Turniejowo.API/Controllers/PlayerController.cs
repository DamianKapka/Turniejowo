using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;
using Turniejowo.API.Services;

namespace Turniejowo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITeamRepository teamRepository;
        private readonly IPlayerRepository playerRepository;

        public PlayerController(IUnitOfWork unitOfWork,ITeamRepository teamRepository,
                                IPlayerRepository playerRepository)
        {
            this.unitOfWork = unitOfWork;
            this.teamRepository = teamRepository;
            this.playerRepository = playerRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var playerToFind = await playerRepository.GetById(id);

                if (playerToFind == null)
                {
                    return NotFound();
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

                var teamForPlayer = await teamRepository.GetById(player.TeamId);

                if (teamForPlayer == null)
                {
                    return NotFound();
                }

                var playerNameExistsForTeam =
                    await playerRepository.FindSingle(p => p.TeamId == player.TeamId && p.FName == player.FName && p.LName == player.LName);

                if (playerNameExistsForTeam != null)
                {
                    return Conflict();
                }

                playerRepository.Add(player);
                await unitOfWork.CompleteAsync();

                return CreatedAtAction("GetById", new { id = player.TeamId }, player);
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

                playerRepository.Update(player);
                await unitOfWork.CompleteAsync();

                return Accepted();
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer([FromRoute] int id)
        {
            try
            {
                var playerToDelete = await playerRepository.GetById(id);

                if (playerToDelete == null)
                {
                    return NotFound();
                }

                playerRepository.Delete(playerToDelete);
                await unitOfWork.CompleteAsync();

                return Accepted();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }       
    }
}