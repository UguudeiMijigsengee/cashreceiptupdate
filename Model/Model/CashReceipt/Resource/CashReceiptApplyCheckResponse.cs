using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.CashReceipt.Resource
{
    public class CashReceiptApplyCheckResponse
    {        
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public string InvoiceDate { get; set; }
    }
}
