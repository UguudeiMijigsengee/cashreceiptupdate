using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.CashReceipt.Resource
{
    public class CashReceiptApply2InvoiceResponse
    {
        public bool isApplied { get; set; }
        public List<string> message { get; set; }
        public CashReceiptApply2InvoiceResponse()
        {
            message = new List<string>();
            isApplied = true;
        }
    }
}
