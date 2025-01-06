using Purchasing.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Purchasing.Domain.Models
{

    public class PurchaseOrder
    {
        private static readonly Random _random = new Random();

        public string POnumber { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTime IssuedDate { get; private set; } = DateTime.UtcNow;
        public PurchaseOrderState State { get; private set; } = PurchaseOrderState.Created;
        public PurchaseOrderActivationState ActivationState { get; private set; } = PurchaseOrderActivationState.Activated;
        public List<PurchaseOrderItem> Items { get; private set; } = new List<PurchaseOrderItem>();

        public PurchaseOrder()
        {
            POnumber = GenerateCode();
            TotalPrice = RecalculateTotalAmount();
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

        public void Deactivate()
        {
            ActivationState = PurchaseOrderActivationState.Deactivated;
        }

        public void AddItem(string Code,int quantity)
        {
            var item = Items.FirstOrDefault(item => item.Code == Code);
            if (item != null)
            {
                throw new InvalidOperationException("Duplicate items are not allowed in a purchase order.");
            }

            Items.Add(item);
            TotalPrice = RecalculateTotalAmount();
        }

        private decimal RecalculateTotalAmount()
        {
            foreach (var item in Items)
            {
                TotalPrice += item.Price;
            }
            return TotalPrice;
        }


        public void UpdateDetails(string orderNumber, DateTime date, decimal totalPrice)
        {
            POnumber = orderNumber;
            IssuedDate = date;
            TotalPrice = totalPrice;
        }

        public void ClearItems()
        {
            Items.Clear();
        }

        private string GenerateCode()
        {
            // Generate a random 7-digit number
            int randomNumber = _random.Next(1000000, 10000000); // Range: 1000000 to 9999999
            return $"PO{randomNumber}";
        }
    }



    // Business logic methods for state transitions
}


