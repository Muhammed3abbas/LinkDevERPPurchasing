using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Domain.DTOs
{
    public class PurchaseOrderDTO
    {
        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public List<PurchaseOrderItemBuyDTO> Items { get; set; }
    }
}
