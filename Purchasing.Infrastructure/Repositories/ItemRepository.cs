using Microsoft.EntityFrameworkCore;
using Purchasing.Domain.Interfaces;
using Purchasing.Domain.Models;
using Purchasing.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly PurchasingDbContext _context;

        public ItemRepository(PurchasingDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseOrderItem> GetByIdAsync(string id) =>
            await _context.PurchaseOrderItems.FirstOrDefaultAsync(i => i.Code == id && !i.IsDeleted);

        public async Task<IEnumerable<PurchaseOrderItem>> GetAllAsync() =>
            await _context.PurchaseOrderItems.AsNoTracking().Where(i => !i.IsDeleted).ToListAsync();

        public async Task AddAsync(PurchaseOrderItem item)
        {
            await _context.PurchaseOrderItems.AddAsync(item);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(PurchaseOrderItem item)
        {
            _context.PurchaseOrderItems.Update(item);
            await SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(PurchaseOrderItem item)
        {

            item.MarkAsDeleted();
            _context.PurchaseOrderItems.Update(item);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }



        public async Task<(List<PurchaseOrderItem>, int)> GetPagedItemsAsync(int pageNumber, int pageSize, string? nameFilter)
        {
            var query = _context.PurchaseOrderItems.AsNoTracking().Where(i => !i.IsDeleted);

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                query = query.Where(i => i.Name.Contains(nameFilter));
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }


        public void Detach(PurchaseOrderItem entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
