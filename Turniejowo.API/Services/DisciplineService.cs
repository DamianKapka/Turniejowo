using System;
using System.Threading.Tasks;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Services
{
    public class DisciplineService : IDisciplineService
    {
        private readonly IDisciplineRepository disciplineRepository;
        private readonly IUnitOfWork unitOfWork;

        public DisciplineService(IDisciplineRepository disciplineRepository, IUnitOfWork unitOfWork)
        {
            this.disciplineRepository = disciplineRepository;
            this.unitOfWork = unitOfWork;
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

        public async Task AddNewDisciplineAsync(Discipline discipline)
        {
            if (await disciplineRepository.FindSingleAsync(d => d.Name == discipline.Name) != null)
            {
                throw new AlreadyInDatabaseException();
            }

            disciplineRepository.Add(discipline);
            await unitOfWork.CompleteAsync();
        }
    }
}
