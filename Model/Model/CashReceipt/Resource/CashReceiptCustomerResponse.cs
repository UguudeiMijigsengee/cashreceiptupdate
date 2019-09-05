using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.CashReceipt.Resource
{
    public class CashReceiptCustomerResponse
    {
        public string custID { get; set; }
        public List<CashReceiptApplyCheckResponse> CashReceiptApplyCheckResponses { get; set; }
    }
}
