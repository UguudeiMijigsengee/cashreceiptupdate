using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink
{
    public class RetailLinkResponse2
    {
        public bool isLoggedIn { get; set; }
        public List<string> message { get; set; }

        public List<RetailLinkResponse> loads { get; set; }
        public RetailLinkResponse2()
        {
            isLoggedIn = true;
            message = new List<string>();
            loads = new List<RetailLinkResponse>();
        }
    }
}
