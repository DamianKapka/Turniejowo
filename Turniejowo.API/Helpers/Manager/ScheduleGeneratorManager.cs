using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Helpers.Manager
{
    public class ScheduleGeneratorManager : IScheduleGeneratorManager
    {
        public async Task<List<DateTime>> GetPossibleMatchDateTimesAsync(GeneratorScheduleOutlines outlines)
        {
            return await Task.Run(() =>
            {

                var startDateTime = DateTime.Parse(outlines.StartDate);
                var endDateTime = DateTime.Parse(outlines.EndDate);

                var list = new List<DateTime>();

                while (startDateTime <= endDateTime)
                {
                    {
                        if (outlines.DaysOfWeek.Contains((int) startDateTime.DayOfWeek))
                        {
                            for (int i = int.Parse(outlines.StartTime.Split(':')[0]);
                                i <= int.Parse(outlines.EndTime.Split(':')[0]);
                                i++)
                            {
                                for (int j = 0; j < outlines.SimultaneousMatches; j++)
                                {
                                    list.Add(new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, i,
                                        0, 0));
                                }
                            }
                        }
                    }
                    startDateTime = startDateTime.AddDays(1);
                }

                return list;
            });
        }

        public async Task<List<Match>> GetPossibleMatchScenarios(List<Team> teams)
        {
            throw new NotImplementedException();
        }
    }
}
