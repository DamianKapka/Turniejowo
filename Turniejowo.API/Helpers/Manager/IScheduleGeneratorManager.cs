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
        Task<Dictionary<int, List<int>>> GetPossibleMatchMatrix(List<Team> teams);
        Task<List<Match>> GenerateSchedule(bool isBracket,List<DateTime> dateTimes, Dictionary<int, List<int>> matchMatrix);
    }
}
