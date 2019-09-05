using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.CashReceipt
{
    public class CashReceiptApplyCheck
    {
        public string MBNumber { get; set; }
        public string Custid { get; set; }
        public string CustName { get; set; }
        public string CusAddress { get; set; }
        public string CityName { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        [Key]
        public string TMWInvoicenumber { get; set; }
        public decimal InvoiceAmt { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
