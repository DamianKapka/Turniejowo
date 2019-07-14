using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService playerService;

        public PlayerController(IPlayerService playerService)
        {
            this.playerService = playerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var player = await playerService.GetPlayerById(id);

                return Ok(player);
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
        public async Task<IActionResult> AddNewPlayer([FromBody] Player player)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await playerService.AddNewPlayer(player);

                return CreatedAtAction("GetById", new {id = player.TeamId}, player);
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
        public async Task<IActionResult> UpdatePlayer([FromRoute] int id, [FromBody] Player player)
        {
            try
            {
                if (id != player.PlayerId)
                {
                    return Conflict();
                }

                await playerService.EditPlayer(player);

                return Accepted();
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
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
                await playerService.DeletePlayer(id);

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