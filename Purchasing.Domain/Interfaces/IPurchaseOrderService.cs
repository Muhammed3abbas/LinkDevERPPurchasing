using Purchasing.Domain.DTOs;
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
        Task<PurchaseOrderReadDTO> UpdatePurchaseOrderAsync(string PONumber, List<PurchaseOrderItemBuyDTO> items);
        Task<bool> DeletePurchaseOrderAsync(string PONumber);
        Task<bool> ApprovePurchaseOrderAsync(string PONumber);
        Task<bool> ShipPurchaseOrderAsync(string PONumber);
        Task<bool> ClosePurchaseOrderAsync(string PONumber);
        Task<bool> DeactivatePurchaseOrderAsync(string PONumber);
        Task<List<PurchaseOrderReadDTO>> GetAllPurchaseOrdersAsync();
        Task<PaginatedResultDTO<PurchaseOrderReadDTO>> GetPagedPurchaseOrdersAsync(int pageNumber, int pageSize, string? POnumberFilter);
        Task<(List<PurchaseOrderReadDTO>, int)> GetCachedPurchaseOrdersAsync();
        Task<PaginatedResultDTO<PurchaseOrderReadDTO>> GetPurchaseOrdersCachedPaginationAsync(int pageNumber, int pageSize);

    }
}
