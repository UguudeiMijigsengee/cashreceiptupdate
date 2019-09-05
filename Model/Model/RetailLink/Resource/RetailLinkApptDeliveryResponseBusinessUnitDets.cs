using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkApptDeliveryResponseBusinessUnitDets
    {
        public string address1 { get; set; }
        public string buId { get; set; }
        public RetailLinkApptDeliveryResponseBusinessUnitDetsBusinessUnitConfigurationBO businessUnitConfigurationBO { get; set; }
        public string city { get; set; }
        public RetailLinkApptDeliveryResponseBusinessUnitDetsCompanyBO companyBO { get; set; }
        public string companyId { get; set; }
        public string contactNumber { get; set; }
        public RetailLinkApptDeliveryResponseBusinessUnitDetsCountryBO countryBO { get; set; }
        public string countryId { get; set; }
        public string createdBy { get; set; }
        public string createdTs { get; set; }
        public string description { get; set; }
        public string lastUpdatedBy { get; set; }
        public string lastUpdatedTs { get; set; }
        public string name { get; set; }
        public List<RetailLinkApptDeliveryResponseBusinessUnitDetsNodeList> nodeList { get; set; }
        public string obsoleteIndicator { get; set; }
        public RetailLinkApptDeliveryResponseBusinessUnitDetsStateBO stateBO { get; set; }
        public string stateId { get; set; }
        public string zipcode { get; set; }
    }
}
