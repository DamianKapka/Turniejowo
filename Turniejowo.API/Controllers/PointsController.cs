using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Turniejowo.API.Models;
using Turniejowo.API.Services;

namespace Turniejowo.API.Controllers
{
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
            throw new NotImplementedException();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditPointsForMatch([FromRoute] int matchId, [FromBody] ICollection<Points> points)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePointsForMatch([FromRoute] int matchId)
        {
            throw new NotImplementedException();
        }
    }
}
