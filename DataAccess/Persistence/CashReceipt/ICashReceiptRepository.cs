using Model.Model.CashReceipt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Persistence.CashReceipt
{
    public interface ICashReceiptRepository
    {
        Task<List<CashReceiptApplyCheck>> getMbInvoices(string query);
    }
}
