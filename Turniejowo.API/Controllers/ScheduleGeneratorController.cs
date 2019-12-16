using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turniejowo.API.Contracts.Requests;
using Turniejowo.API.Models;
using Turniejowo.API.Services;

namespace Turniejowo.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleGeneratorController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IScheduleGeneratorService scheduleGeneratorService;

        public ScheduleGeneratorController(IMapper mapper, IScheduleGeneratorService scheduleGeneratorService)
        {
            this.mapper = mapper;
            this.scheduleGeneratorService = scheduleGeneratorService;
        }

        //TO DEL
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] ScheduleGeneratorRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await scheduleGeneratorService.GenerateScheduleAsync(mapper.Map<GeneratorScheduleOutlines>(request));

                return Ok("dasdasdas");
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
            catch (DataMisalignedException)
            {
                return Conflict();
            }
            catch (FormatException)
            {
                return StatusCode(406);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
