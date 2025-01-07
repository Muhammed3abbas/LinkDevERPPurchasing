using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Domain.Models
{
    public class PurchaseOrderItemMapping
    {
        public int SerialNumber { get; set; } // PK for this table
        public int Quantity { get; set; } // PK for this table

        // Foreign keys
        public string PurchaseOrderPOnumber { get; set; }
        public string PurchaseOrderItemCode { get; set; }

        // Navigation properties
        public PurchaseOrder PurchaseOrder { get; set; }
        public PurchaseOrderItem PurchaseOrderItem { get; set; }
    }

}
