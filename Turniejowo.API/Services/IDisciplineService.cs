using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Services
{
    public interface IDisciplineService
    {
        Task<string> GetDisciplineNameByIdAsync(int id);
        Task<int> GetDisciplineIdByNameAsync(string name);
    }
}
