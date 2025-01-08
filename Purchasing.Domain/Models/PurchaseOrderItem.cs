using Purchasing.Domain.Helper;
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
            Code = CodeGenerator.GeneratePurchaseOrderItemCode();
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


        public void AdjustQuantity(int amount)
        {
            if (Quantity + amount < 0)
            {
                throw new InvalidOperationException("Insufficient quantity available.");
            }
            Quantity += amount;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }

    }




}
