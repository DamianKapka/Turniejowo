using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Models;
using Turniejowo.API.Services;

namespace Turniejowo.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly IPointsService pointsService;
        private readonly IMapper mapper;

        public PointsController(IPointsService pointsService,IMapper mapper)
        {
            this.pointsService = pointsService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddPointsForMatch([FromBody] ICollection<Points> points)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await pointsService.AddPointsForMatchAsync(points);

                return Accepted();
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditPointsForMatch([FromRoute] int matchId, [FromBody] ICollection<Points> points)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await pointsService.EditPointsForMatchAsync(points);

                return Accepted();
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePointsForMatch([FromRoute] int matchId)
        {
            try
            {
                await pointsService.DeletePointsForMatchAsync(matchId);

                return Accepted();
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
