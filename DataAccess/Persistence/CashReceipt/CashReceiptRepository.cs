using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.Model.CashReceipt;

namespace DataAccess.Persistence.CashReceipt
{
    public class CashReceiptRepository : ICashReceiptRepository
    {
        private readonly RJWDataDbContext context;

        public CashReceiptRepository(RJWDataDbContext context)
        {
            this.context = context;
        }

        public async Task<List<CashReceiptApplyCheck>> getMbInvoices(string query)
        {
            try
            {
            var invoices = await context.cashReceiptApplyChecks.FromSql(query).ToListAsync();
            return invoices;

            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
