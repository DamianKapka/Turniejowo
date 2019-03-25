using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var tournament = await _tournamentRepository.GetById(id);
            return Ok(tournament);
        }



    }
}