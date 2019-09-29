using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface IDisciplineService
    {
        Task<string> GetDisciplineNameByIdAsync(int id);
        Task<int> GetDisciplineIdByNameAsync(string name);
        Task AddNewDisciplineAsync(Discipline discipline);
    }
}
