using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;
using Turniejowo.API.Services;

namespace Turniejowo.API.Controllers
{
    [Authorize]
    [Route("/api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly ITeamRepository teamRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IUnitOfWork unitOfWork;

        public MatchController(IMatchRepository matchRepository,IUnitOfWork unitOfWork,
                               ITeamRepository teamRepository)
        {
            this.matchRepository = matchRepository;
            this.unitOfWork = unitOfWork;
            this.teamRepository = teamRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var matches = await matchRepository.GetAll();

                if (matches == null)
                {
                    return NotFound();
                }

                return Ok(matches);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var match = await matchRepository.FindSingle(m => m.MatchId == id);

                if(match == null)
                {
                    return NotFound();
                }

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
                    return BadRequest(ModelState);
                }

                if (await teamRepository.FindSingle(x => x.TeamId == match.HomeTeamId) == null || await teamRepository.FindSingle(y => y.TeamId == match.GuestTeamId) == null)
                {
                    return NotFound();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id,[FromBody] Match match)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var matchToEdit = await Task<Match>.Run(() => matchRepository.FindSingle(x => x.MatchId == id));

                if (matchToEdit == null)
                {
                    return NotFound();
                }

                matchRepository.ClearEntryState(matchToEdit);

                matchRepository.Update(match);
                await unitOfWork.CompleteAsync();

                return Accepted();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var matchToDel = await matchRepository.FindSingle(x => x.MatchId == id);

                if (matchToDel == null)
                {
                    return NotFound();
                }

                matchRepository.Delete(matchToDel);
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
