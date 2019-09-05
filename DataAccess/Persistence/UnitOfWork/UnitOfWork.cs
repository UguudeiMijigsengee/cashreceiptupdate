using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RJWDataDbContext context;

        public UnitOfWork(RJWDataDbContext context)
        {
            this.context = context;
        }
        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
