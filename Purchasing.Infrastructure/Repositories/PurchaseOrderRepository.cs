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
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly PurchasingDbContext _context;

        public PurchaseOrderRepository(PurchasingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllAsync()
        {
            return await _context.PurchaseOrders
            .Include(po => po.PurchaseOrderItemMappings)
            .ThenInclude(mapping => mapping.PurchaseOrderItem)
                .Where(po => !po.IsDeleted)
                .OrderByDescending(po=>po.IssuedDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.PurchaseOrders.CountAsync(po => !po.IsDeleted);
        }

        public async Task<(IEnumerable<PurchaseOrder> Items, int TotalCount)> GetPagedItemsAsync(
            int pageNumber,
            int pageSize,
            string? POnumberFilter)
        {
            // Base query
            var query = _context.PurchaseOrders
                .Include(po => po.PurchaseOrderItemMappings)
                .ThenInclude(mapping => mapping.PurchaseOrderItem)
                .OrderByDescending(po => po.IssuedDate)
                .Where(po => !po.IsDeleted)
                .AsNoTracking();

            // Apply filter if provided
            if (!string.IsNullOrWhiteSpace(POnumberFilter))
            {
                query = query.Where(po => po.POnumber.Contains(POnumberFilter));
            }

            // Get the total count before applying pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var pagedItems = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return both paged items and total count
            return (pagedItems, totalCount);
        }



        public async Task<List<PurchaseOrder>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.PurchaseOrders
                .Include(po => po.PurchaseOrderItemMappings)
                .ThenInclude(mapping => mapping.PurchaseOrderItem)
                .Where(po => !po.IsDeleted)
                .OrderByDescending(po => po.IssuedDate)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.PurchaseOrders.CountAsync(po => !po.IsDeleted);
        }

        public async Task<PurchaseOrder> GetByIdAsync(string PONumber)
        {


            return await _context.PurchaseOrders
            .Include(po => po.PurchaseOrderItemMappings)
            .ThenInclude(mapping => mapping.PurchaseOrderItem) 
            .FirstOrDefaultAsync(po => po.POnumber == PONumber && !po.IsDeleted);
        }



        public async Task AddAsync(PurchaseOrder purchaseOrder)
        {
            await _context.PurchaseOrders.AddAsync(purchaseOrder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Update(purchaseOrder);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string PONumber)
        {
            var purchaseOrder = await GetByIdAsync(PONumber);
            if (purchaseOrder == null) return false;

            purchaseOrder.MarkAsDeleted();
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
