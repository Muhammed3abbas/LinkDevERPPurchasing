using Purchasing.Domain.Enums;
using Purchasing.Domain.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Purchasing.Domain.Models
{

    public class PurchaseOrder : BaseEntity
    {

        public string POnumber { get; private set; }
        public decimal TotalPrice { get;  set; }
        public DateTime IssuedDate { get; private set; } = DateTime.UtcNow;
        public PurchaseOrderState State { get; private set; } = PurchaseOrderState.Created;
        public PurchaseOrderActivationState ActivationState { get; private set; } = PurchaseOrderActivationState.Activated;
        public List<PurchaseOrderItem> Items { get; private set; } = new List<PurchaseOrderItem>();

        // Navigation property for the junction table
        public List<PurchaseOrderItemMapping> PurchaseOrderItemMappings { get; private set; } = new List<PurchaseOrderItemMapping>();


        public PurchaseOrder()
        {
            POnumber = CodeGenerator.GeneratePurchaseOrderCode();
        }

        public void Approve()
        {
            if (State != PurchaseOrderState.Created)
            {
                throw new InvalidOperationException("Only a created order can be approved.");
            }
            State = PurchaseOrderState.Approved;
        }

        public void Ship()
        {
            if (State != PurchaseOrderState.Approved)
            {
                throw new InvalidOperationException("Only an approved order can be shipped.");
            }
            State = PurchaseOrderState.Shipped;
        }

        public void Close()
        {
            if (State != PurchaseOrderState.Shipped)
            {
                throw new InvalidOperationException("Only a shipped order can be closed.");
            }
            State = PurchaseOrderState.Closed;
        }


        public void Deactivate() => ActivationState = PurchaseOrderActivationState.Deactivated;

        public void MarkAsDeleted() => IsDeleted = true;

        public void ClearItems() => Items.Clear();

        public void UpdateDetails(string orderNumber,  decimal totalPrice)
        {
            POnumber = orderNumber;
            TotalPrice = totalPrice;
        }



    }

}


