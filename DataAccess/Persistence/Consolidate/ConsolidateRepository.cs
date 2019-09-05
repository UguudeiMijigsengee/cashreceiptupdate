using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.Model.Consolidate;

namespace DataAccess.Persistence.Consolidate
{
    public class ConsolidateRepository : IConsolidateRepository
    {
        private readonly RJWDataDbContext context;

        public ConsolidateRepository(RJWDataDbContext context)
        {
            this.context = context;
        }

        public async Task runConsolidateMergeMain(string mainSP)
        {           
            await context.Database.ExecuteSqlCommandAsync(mainSP);            
        }

        public async Task<List<ConsolidateErrorLogTable>> runConsolidateErrorTb(string errorQuery)
        {
            var consolidateErrorLogTables = await context.consolidateErrorLogTables.FromSql(errorQuery).ToListAsync();
            return consolidateErrorLogTables;
        }

        public async Task<List<ConsolidateSuccessLogTable>> runConsolidateSuccessTb(string successQuery)
        {
            var consolidateSuccessLogTables = await context.consolidateSuccessLogTables.FromSql(successQuery).ToListAsync();
            return consolidateSuccessLogTables;
        }
    }
}
