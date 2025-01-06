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
            return await _context.PurchaseOrders.ToListAsync();
        }

        public async Task<PurchaseOrder> GetByIdAsync(string PONumber)
        {
            return await _context.PurchaseOrders.Include(po => po.Items).FirstOrDefaultAsync(po => po.POnumber == PONumber);
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

        public async Task DeleteAsync(string PONumber)
        {
            var purchaseOrder = await GetByIdAsync(PONumber);
            if (purchaseOrder != null)
            {
                _context.PurchaseOrders.Remove(purchaseOrder);
                await _context.SaveChangesAsync();
            }
        }
    }
}
