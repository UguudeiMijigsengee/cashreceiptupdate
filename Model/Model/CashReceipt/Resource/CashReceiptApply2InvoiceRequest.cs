using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.CashReceipt.Resource
{
    public class CashReceiptApply2InvoiceRequest
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        public string CustID { get; set; }
        [Required(ErrorMessage = "Check # is required.")]
        public string CheckNumber { get; set; }
        [Required(ErrorMessage = "Check Date is required.")]
        public string CheckDate { get; set; }
        [Required(ErrorMessage = "Check Amount is required.")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Company is required.")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Cash Receipt Type is required.")]
        public string CashReceiptType { get; set; }
        public List<CashReceiptApply2Invoice> Apply2Invoices { get; set; }
    }
}
