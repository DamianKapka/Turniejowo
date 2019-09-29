using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Models;
using Turniejowo.API.Services;

namespace Turniejowo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplineController : ControllerBase
    {
        private readonly IDisciplineService disciplineService;

        public DisciplineController(IDisciplineService disciplineService)
        {
            this.disciplineService = disciplineService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscipline([FromQuery] int id, [FromQuery] string name)
        {
            try
            {
                if (id != 0 ^ name != null)
                {
                    dynamic disciplineInfo;

                    if (id != 0)
                    {
                        disciplineInfo = await disciplineService.GetDisciplineNameByIdAsync(id);
                    }
                    else
                    {
                        disciplineInfo = await disciplineService.GetDisciplineIdByNameAsync(name);
                    }

                    return Ok(disciplineInfo);
                }

                return BadRequest("You either have to provide id or name");
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

        [HttpPost]
        public async Task<IActionResult> AddDiscipline([FromBody] Discipline discipline)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await disciplineService.AddNewDisciplineAsync(discipline);
                return CreatedAtAction("GetDiscipline", new {id = discipline.DisciplineId}, discipline);
            }
            catch (AlreadyInDatabaseException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
