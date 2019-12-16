using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Helpers.Manager
{
    public interface IScheduleGeneratorManager
    {
        Task<List<DateTime>> GetPossibleMatchDateTimesAsync(GeneratorScheduleOutlines outlines);
        Task<List<Match>> GetPossibleMatchScenarios(List<Team> teams);
    }
}
