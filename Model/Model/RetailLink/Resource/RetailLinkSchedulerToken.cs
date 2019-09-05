using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkSchedulerToken
    {
        public string token { get; set; }
        public string securityID { get; set; }
        public string userId { get; set; }       
        public string userFirstName { get; set; }       
        public string userLastName { get; set; }       
        public string userType { get; set; }       
        public string loginId { get; set; }
        public string langCode { get; set; }
        public List<Cookie> cookies { get; set; }

        public RetailLinkSchedulerToken()
        {
            cookies = new List<Cookie>();
        }
    }
}
