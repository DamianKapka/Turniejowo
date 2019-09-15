using System;
using System.Threading.Tasks;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Repositories;

namespace Turniejowo.API.Services
{
    public class DisciplineService : IDisciplineService
    {
        private readonly IDisciplineRepository disciplineRepository;

        public DisciplineService(IDisciplineRepository disciplineRepository)
        {
            this.disciplineRepository = disciplineRepository;
        }

        public async Task<string> GetDisciplineNameByIdAsync(int id)
        {
            var discipline = await disciplineRepository.FindSingleAsync(d => d.DisciplineId == id);

            if (discipline == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return discipline.Name;
        }

        public async Task<int> GetDisciplineIdByNameAsync(string name)
        {
            var discipline = await disciplineRepository.FindSingleAsync(d => d.Name == name);

            if (discipline == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return discipline.DisciplineId;
        }
    }
}
