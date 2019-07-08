using System.Threading.Tasks;

namespace Turniejowo.API.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
