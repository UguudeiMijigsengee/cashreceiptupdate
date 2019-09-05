using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkUserResponse
    {
        public string activeIndicator { get; set; }
        public string domainName { get; set; }
        public string email { get; set; }
        public int langCode { get; set; }
        public string languageDesc { get; set; }
        public string lastName { get; set; }
        public string obsoleteIndicator { get; set; }
        public string organization { get; set; }
        public string primaryPhoneNbr { get; set; }
        public string userId { get; set; }
        public string userPrincipleNbr { get; set; }
        public List<RetailLinkUserRole> userRoles { get; set; }
        public RetailLinkUserType userType { get; set; }
        public List<RetailLinkUserbuList> userbuList { get; set; }
        public int visitCount { get; set; }

        public RetailLinkUserResponse()
        {
            userRoles = new List<RetailLinkUserRole>();
        }
    }
}
