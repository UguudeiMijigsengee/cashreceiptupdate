using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
