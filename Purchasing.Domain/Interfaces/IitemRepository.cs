using Purchasing.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Domain.Interfaces
{
    public interface IItemRepository
    {
        Task<PurchaseOrderItem> GetByIdAsync(string id);
        Task<IEnumerable<PurchaseOrderItem>> GetAllAsync();
        //Task<IEnumerable<PurchaseOrderItem>> GetPagedItemsAsync(int pageNumber, int pageSize, string? nameFilter);
        Task<(List<PurchaseOrderItem>, int)> GetPagedItemsAsync(int pageNumber, int pageSize, string? nameFilter);

        Task AddAsync(PurchaseOrderItem item);
        Task UpdateAsync(PurchaseOrderItem item);
        Task SoftDeleteAsync(PurchaseOrderItem item);
        Task SaveChangesAsync();
        void Detach(PurchaseOrderItem entity);





    }
}
