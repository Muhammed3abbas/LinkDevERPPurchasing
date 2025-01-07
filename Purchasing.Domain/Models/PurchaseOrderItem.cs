using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Purchasing.Domain.Models
{
    public class PurchaseOrderItem : BaseEntity
    {
        private static readonly Random _random = new Random();

        public string Code { get;  set; } 
        public string Name { get;  set; }
        public decimal Price { get;  set; }
        public int Quantity { get;  set; }

        // Navigation property for the junction table
        public List<PurchaseOrderItemMapping> PurchaseOrderItemMappings { get; private set; } = new List<PurchaseOrderItemMapping>();
        public PurchaseOrderItem()
        {
                
        }


        public PurchaseOrderItem(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            IsDeleted = false;
            Code = GenerateCode();
            Quantity = quantity;

        }

        public PurchaseOrderItem(string name, decimal price, int quantity,string code)
        {
            Name = name;
            Price = price;
            IsDeleted = false;
            Code = code;
            Quantity = quantity;

        }

        //internal void AssignSerialNumber(int serialNumber)
        //{
        //    SerialNumber = serialNumber; 
        //}

        public void AdjustQuantity(int amount)
        {
            if (Quantity + amount < 0)
            {
                throw new InvalidOperationException("Insufficient quantity available.");
            }
            Quantity += amount;
        }
        public void Update(string? name, decimal? price , int? quantity)
        {
            Name = string.IsNullOrWhiteSpace(name) ? Name : name;
            Price = price ?? Price;
            Quantity = quantity ?? Quantity;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }

        private string GenerateCode()
        {
            // Generate a random 7-digit number
            int randomNumber = _random.Next(1000000, 10000000); // Range: 1000000 to 9999999
            return $"POI{randomNumber}";
        }
    }




}
