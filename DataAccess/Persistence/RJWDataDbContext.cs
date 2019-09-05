using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.Model;
using Model.Model.Consolidate;
using Model.Model.Log;
using Model.Model.RetailLink;
using Model.Model.CashReceipt;

namespace DataAccess.Persistence
{
    public class RJWDataDbContext : DbContext
    {
        public DbSet<RJWLoadsRetailApptMultipleRJWTool> RJWLoadsRetailApptMultipleRJWTools { get; set; }
        public DbSet<tblProcessErrorLog> tblProcessErrorLogs { get; set; }
        public DbSet<RetailLinkLoad> retailLinkLoads { get; set; }
        public DbSet<RJWLoad> rjwLoads { get; set; }
        public DbSet<ConsolidateErrorLogTable> consolidateErrorLogTables { get; set; }
        public DbSet<ConsolidateSuccessLogTable> consolidateSuccessLogTables { get; set; }
        public DbSet<CashReceiptApplyCheck> cashReceiptApplyChecks { get; set; }
        public RJWDataDbContext(DbContextOptions<RJWDataDbContext> options, IOptions<Config> config) : base(options)
        {
            Database.SetCommandTimeout(config.Value.SQLServerTimeOut);
        }
    }
}
