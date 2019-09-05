using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkAvailableAppWindowsRequestShipment
    {
        public string convInd { get; set; }
        public string cutOffDateInStrFmt { get; set; }
        public int inventoryTypeId { get; set; }
        public bool isDummy { get; set; }
        public int loadTypeId { get; set; }
        public string paymentTerm { get; set; }
        public int poCaseQty { get; set; }
        public string poEvent { get; set; }
        public string specialPoType { get; set; }
        public string vendorDepartmentNum { get; set; }
        public string vendorId { get; set; }
        public string vendorSequenceNum { get; set; }
    }
}
