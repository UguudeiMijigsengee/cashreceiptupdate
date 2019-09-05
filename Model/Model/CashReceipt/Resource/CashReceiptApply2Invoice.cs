using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.CashReceipt.Resource
{
    public class CashReceiptApply2Invoice
    {
        [Required(ErrorMessage = "Invoice # is required.")]
        public string InvoiceNumber { get; set; }
        [Required(ErrorMessage = "Invoice Amount is required.")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Check Date is required.")]
        public string CheckDate { get; set; }
        [Required(ErrorMessage = "Apply From Doc Type is required.")]
        public string applyFromDocType { get; set; }
        [Required(ErrorMessage = "Apply To Doc Type is required.")]
        public string applyToDocType { get; set; }
    }
}
