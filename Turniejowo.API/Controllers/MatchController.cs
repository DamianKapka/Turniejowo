using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;

namespace Turniejowo.API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchRepository matchRepository;

        public MatchController(IMatchRepository matchRepository)
        {
            this.matchRepository = matchRepository;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var matches = await matchRepository.GetAll();

                if (matches == null)
                {
                    return NoContent();
                }

                return Ok(matches);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var match = await matchRepository.FindSingle(m => m.MatchId == id);

                return Ok(match);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
