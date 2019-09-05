using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.CashReceipt.Resource
{
    public class CashReceiptRequest
    {
        [Required(ErrorMessage = "Company is required.")]
        public string Company { get; set; }
        [Required(ErrorMessage = "MB# is required.")]
        public string MbNumber { get; set; }
    }
}
