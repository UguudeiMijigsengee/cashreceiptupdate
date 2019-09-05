using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkApptDeliveryResponseDestinationNodeDets
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string buId { get; set; }
        public RetailLinkApptDeliveryResponseDestinationNodeDetsBusinessUnitDets businessUnitDets { get; set; }
        public string city { get; set; }
        public string contactName { get; set; }
        public string contactNumber { get; set; }
        public RetailLinkApptDeliveryResponseDestinationNodeDetsCountryDets countryDets { get; set; }
        public string countryId { get; set; }
        public string createdBy { get; set; }
        public string createdTs { get; set; }
        public string email { get; set; }
        public bool flag { get; set; }
        public string globalLocNbr { get; set; }
        public string lastUpdatedBy { get; set; }
        public long lastUpdatedTs { get; set; }
        public RetailLinkApptDeliveryResponseDestinationNodeDetsNodeConfigurationBO nodeConfigurationBO { get; set; }
        public string nodeDesc { get; set; }
        public string nodeId { get; set; }
        public List<object> nodeInventoryConfiguration { get; set; }
        public string nodeName { get; set; }
        public string obsoleteIndicator { get; set; }
        public string sbIndicator { get; set; }
        public RetailLinkApptDeliveryResponseDestinationNodeDetsStateDets stateDets { get; set; }
        public int stateId { get; set; }
        public List<object> subNodeList { get; set; }
        public RetailLinkApptDeliveryResponseDestinationNodeDetsTimeZoneDets timeZoneDets { get; set; }        
        public string timezoneId { get; set; }
        public string zipCode { get; set; }        
    }
}
