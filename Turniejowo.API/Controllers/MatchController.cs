using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var matches = await matchRepository.GetAll();

            if(matches == null)
            {
                return NoContent();
            }

            return Ok(matches);
        }
    }
}
