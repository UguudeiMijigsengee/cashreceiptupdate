using Model.Model.CashReceipt.Resource;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.CashReceipt.Resource
{
    public class CashReceiptResponse
    {
        public bool isInvoiced { get; set; }
        public List<string> message { get; set; }
        public CashReceiptCustomerResponse CashReceiptCustomerResponse { get; set; }

        public CashReceiptResponse()
        {
            message = new List<string>();
            isInvoiced = true;
            CashReceiptCustomerResponse = new CashReceiptCustomerResponse();
        }
    }
}
