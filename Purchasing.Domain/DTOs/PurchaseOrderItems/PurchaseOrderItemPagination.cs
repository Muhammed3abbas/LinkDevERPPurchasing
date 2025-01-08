using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Domain.DTOs.PurchaseOrderItems
{
    public class PurchaseOrderItemPagination
    {
        public string? nameFilter { get; set; } = null;
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}
