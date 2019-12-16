using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface IScheduleGeneratorService
    {
        Task GenerateScheduleAsync(GeneratorScheduleOutlines outlines);
    }
}
