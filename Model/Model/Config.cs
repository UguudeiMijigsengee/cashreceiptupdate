using System;
using System.Collections.Generic;
using System.Text;
using Model.Model.Consolidate;
using Model.Model.Login;
using Model.Model.RetailLink;
using Model.Model.SynapseItemGenerator;
using Model.Model.UserService;
using Model.Model.CashReceipt;

namespace Model.Model
{
    public class Config
    {
        public string name { get; set; }
        public string SQLP01_RJWData { get; set; }
        public int SQLServerTimeOut { get; set; }
        public UserServiceConfig UserServiceConfig { get; set; }
        public ConfigRetailLink ConfigRetailLink { get; set; }
        public SynapseItemGeneratorConfig SynapseItemGeneratorConfig { get; set; }
        public ConsolidateConfig ConsolidateConfig { get; set; }
        public CashReceiptConfig CashReceiptConfig { get; set; }
    }
}
