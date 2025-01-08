using Purchasing.Domain.DTOs.PurchaseOrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Domain.DTOs.PurchaseOrder
{
    public class PurchaseOrderUpdateDTO
    {
        public string POnumber { get; set; }
        public List<PurchaseOrderItemBuyDTO> Items { get; set; }
    }
}
