using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Models.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
