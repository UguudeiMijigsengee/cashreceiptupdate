using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink
{
    public class ConfigRetailLink
    {
        public string loadExistQuery { get; set; }
        public string mainQuery { get; set; }
        public List<string> queueStatuses { get; set; }
        public string synapseQuery { get; set; }
        public string fromSameDCPattern { get; set; }        
        public string dropDateFormat { get; set; }
        public string deliveryRequestDateFormat { get; set; }
        public List<string> messages { get; set; }
        public List<string> URLs { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public Dictionary<string, string> formData { get; set; }
        public List<string> tokenParams { get; set; }
        public List<string> headers { get; set; }
    }
}
