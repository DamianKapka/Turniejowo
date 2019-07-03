using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;

namespace Turniejowo.API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly ITeamRepository teamRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IUnitOfWork unitOfWork;

        public MatchController(IMatchRepository matchRepository,IUnitOfWork unitOfWork,ITeamRepository teamRepository)
        {
            this.matchRepository = matchRepository;
            this.unitOfWork = unitOfWork;
            this.teamRepository = teamRepository;
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

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Match match)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Match model invalid");
                }

                if (await teamRepository.FindSingle(x => x.TeamId == match.HomeTeamId) == null || await teamRepository.FindSingle(y => y.TeamId == match.GuestTeamId) == null)
                {
                    return BadRequest("One of teams doeas not exist");
                }

                matchRepository.Add(match);
                await unitOfWork.CompleteAsync();
                return CreatedAtAction("Get",new { id = match.MatchId }, match);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Match match)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Match model invalid");
                }

                if (matchRepository.FindSingle(x => x.MatchId == match.MatchId) == null)
                {
                    return BadRequest("No such match");
                }

                matchRepository.Update(match);
                await unitOfWork.CompleteAsync();

                return Accepted();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    } 
}
