using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.CashReceipt
{
    public class CashReceiptConfig
    {
        public List<string> storedProcedures { get; set; }
        public List<string> messages { get; set; }
        public string dateFormat { get; set; }
        public List<string> GPServiceURLs { get; set; }
        public string contentType { get; set; }
    }
}
