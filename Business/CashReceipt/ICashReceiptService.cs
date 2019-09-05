using Model.Model.CashReceipt.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.CashReceipt
{
    public interface ICashReceiptService
    {
        Task<CashReceiptResponse> getMbInvoices(CashReceiptRequest request, string userName);
        Task<CashReceiptApply2InvoiceResponse> apply2Invoice(CashReceiptApply2InvoiceRequest request, string userName);
    }
}
