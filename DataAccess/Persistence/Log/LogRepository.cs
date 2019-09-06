using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model.Model.Log;

namespace DataAccess.Persistence.Log
{
    public class LogRepository : ILogRepository
    {
        private readonly RJWDataDbContext context;

        public LogRepository(RJWDataDbContext context)
        {
            this.context = context;
        }
        public void Add(tblProcessErrorLog log)
        {
            context.tblProcessErrorLogs.Add(log);
        }

        public async Task<tblProcessErrorLog> GetLog(int id)
        {
            return await context.tblProcessErrorLogs.FindAsync(id);
        }

        public void Remove(tblProcessErrorLog log)
        {
            context.Remove(log);
        }
    }
}
