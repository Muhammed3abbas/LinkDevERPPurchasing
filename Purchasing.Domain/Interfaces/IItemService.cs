using Purchasing.Domain.DTOs.PurchaseOrderItems;
using Purchasing.Domain.DTOs;
using Purchasing.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Domain.Interfaces
{
    public interface IItemService
    {
        Task<PurchaseOrderItemDTO> CreateItemAsync(string name, decimal price, int quantity);
        Task DeleteItemAsync(string code);
        Task<IEnumerable<PurchaseOrderItem>> GetAllItemsAsync();
        Task<PurchaseOrderItem> GetItemByIdAsync(string id);
        Task<PaginatedResultDTO<PurchaseOrderItemDTO>> GetPagedItemsAsync(int pageNumber, int pageSize, string? nameFilter);
        Task<PurchaseOrderItem> UpdateItemAsync(PurchaseOrderItemUpdateDTO itemUpdateDTO);
    }
}
