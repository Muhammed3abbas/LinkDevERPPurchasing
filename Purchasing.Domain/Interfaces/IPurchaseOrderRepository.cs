using Purchasing.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Domain.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder> GetByIdAsync(string PONumber);
        Task<IEnumerable<PurchaseOrder>> GetAllAsync();
        Task AddAsync(PurchaseOrder purchaseOrder);
        Task UpdateAsync(PurchaseOrder purchaseOrder);
        Task<bool> DeleteAsync(string PONumber);


        //Task<PurchaseOrder> CreatePurchaseOrderAsync(List<PurchaseOrderItem> items);
        //Task<PurchaseOrder> GetPurchaseOrderByIdAsync(string PONumber);
        //Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();
        //Task<PurchaseOrder> UpdatePurchaseOrderAsync(string PONumber, string orderNumber, DateTime date, decimal totalPrice, List<PurchaseOrderItemDTO> items);
        //Task<bool> DeletePurchaseOrderAsync(string PONumber);
        //Task<bool> ApprovePurchaseOrderAsync(string PONumber);
        //Task<bool> ShipPurchaseOrderAsync(string PONumber);
        //Task<bool> ClosePurchaseOrderAsync(string PONumber);




    }
}
