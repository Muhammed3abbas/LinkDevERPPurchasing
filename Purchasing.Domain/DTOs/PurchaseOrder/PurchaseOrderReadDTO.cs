

using Purchasing.Domain.DTOs.PurchaseOrderItems;
using Purchasing.Domain.Enums;
using Purchasing.Domain.Models;

namespace Purchasing.Domain.DTOs.PurchaseOrder
{
    public class PurchaseOrderReadDTO
    {
        public string POnumber { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime IssuedDate { get; set; }
        public string State { get; set; } // Map enum to string
        public string ActivationState { get; set; } // Map enum to string
        public List<PurchaseOrderItemReadDto> Items { get; set; } = new List<PurchaseOrderItemReadDto>();
    }
}
