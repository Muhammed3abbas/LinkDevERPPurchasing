using Purchasing.Domain.DTOs.PurchaseOrder;
using Purchasing.Domain.DTOs.PurchaseOrderItems;
using Purchasing.Domain.Enums;
using Purchasing.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Domain.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderReadDTO> CreatePurchaseOrderAsync(List<PurchaseOrderItemBuyDTO> items);
        Task<PurchaseOrderReadDTO> GetPurchaseOrderByIdAsync(string PONumber);
        //Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();
        Task<PurchaseOrder> UpdatePurchaseOrderAsync(string PONumber, string orderNumber, DateTime date, decimal totalPrice, List<PurchaseOrderItemBuyDTO> items);
        Task<bool> DeletePurchaseOrderAsync(string PONumber);
        Task<bool> ApprovePurchaseOrderAsync(string PONumber);
        Task<bool> ShipPurchaseOrderAsync(string PONumber);
        Task<bool> ClosePurchaseOrderAsync(string PONumber);
        Task<bool> DeactivatePurchaseOrderAsync(string PONumber);
        Task<List<PurchaseOrderReadDTO>> GetAllPurchaseOrdersAsync();


    }
}
