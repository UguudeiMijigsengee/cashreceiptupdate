using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.Model.RetailLink
{
    [Table("RJWLoadsRetailApptMultipleRJWTool")]
    public class RJWLoadsRetailApptMultipleRJWTool
    {
        public int ID { get; set; }
        public string USERID { get; set; }
        public Decimal LOADNO { get; set; }
        public DateTime DeliveryDate { get; set; }
        [StringLength(50)]
        public string DeliveryId { get; set; }
        public Decimal RefLOADNO { get; set; }
        public string Status { get; set; }
        public DateTime EntryDate { get; set; }        
        public DateTime WalmartEnforcedDate { get; set; }
        public Decimal NumberofOrdersByUser { get; set; }          
    }
}
