using Purchasing.Domain.Enums;
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
        private static readonly Random _random = new Random();

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
            POnumber = GenerateCode();
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

        //public void AddItem(PurchaseOrderItem item, int quantity)
        //{
        //    if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");



        //        // Assign a unique serial number
        //        int nextSerialNumber = Items.Count > 0 ? Items.Max(i => i.SerialNumber.GetValueOrDefault()) + 1 : 1;
        //        item.AssignSerialNumber(nextSerialNumber);

        //        // Add the new item to the Items list
        //        Items.Add(item);
            

        //    // Recalculate the total price of the purchase order
        //    TotalPrice = RecalculateTotalAmount();

        //}



        //public decimal RecalculateTotalAmount()
        //{
        //    var p = Items.Sum(item => item.Price * item.Quantity);

        //    return p;
        //}

        public void UpdateDetails(string orderNumber, DateTime date, decimal totalPrice)
        {
            POnumber = orderNumber;
            IssuedDate = date;
            TotalPrice = totalPrice;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }

        public void ClearItems()
        {
            Items.Clear();
        }

        private string GenerateCode()
        {
            int randomNumber = _random.Next(1000000, 10000000);
            return $"PO{randomNumber}";
        }
    }

}


