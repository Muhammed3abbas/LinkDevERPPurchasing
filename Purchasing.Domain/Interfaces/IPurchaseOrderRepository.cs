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
        Task DeleteAsync(string PONumber);
    }
}
