using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Purchasing.Domain.Models
{
    public class PurchaseOrderItem
    {
        private static readonly Random _random = new Random();

        public string Code { get; private set; } 
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public bool IsDeleted { get; private set; }


        public PurchaseOrderItem(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            IsDeleted = false;
            Code = GenerateCode();
            Quantity = quantity;

        }


        public void AdjustQuantity(int amount)
        {
            if (Quantity + amount < 0)
            {
                throw new InvalidOperationException("Insufficient quantity available.");
            }
            Quantity += amount;
        }
        public void Update(string code, decimal? price , int? quantity)
        {
            Code = code;
            Price = (decimal)price;
            Quantity = (int)quantity;
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
