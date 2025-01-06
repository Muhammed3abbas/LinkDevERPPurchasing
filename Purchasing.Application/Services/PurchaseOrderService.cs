using Purchasing.Application.DTOs;
using Purchasing.Domain.Enums;
using Purchasing.Domain.Interfaces;
using Purchasing.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Application.Services
{
    public class PurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repository;

        public PurchaseOrderService(IPurchaseOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<PurchaseOrder> CreatePurchaseOrderAsync(List<PurchaseOrderItemDTO> items)
        {
            var purchaseOrder = new PurchaseOrder();

            foreach (var itemDto in items)
            {

                purchaseOrder.AddItem(itemDto.Code, itemDto.Quantity);
            }

            await _repository.AddAsync(purchaseOrder);
            return purchaseOrder;
        }

        public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(string PONumber)
        {
            return await _repository.GetByIdAsync(PONumber);
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PurchaseOrder> UpdatePurchaseOrderAsync(string PONumber, string orderNumber, DateTime date, decimal totalPrice, List<PurchaseOrderItemDTO> items)
        {
            var purchaseOrder = await _repository.GetByIdAsync(PONumber);
            if (purchaseOrder == null)
            {
                return null;
            }

            purchaseOrder.UpdateDetails(orderNumber, date, totalPrice);
            purchaseOrder.ClearItems();

            foreach (var itemDto in items)
            {
                purchaseOrder.AddItem(itemDto.Code, itemDto.Quantity);
            }

            await _repository.UpdateAsync(purchaseOrder);
            return purchaseOrder;
        }

        public async Task<bool> DeletePurchaseOrderAsync(string PONumber)
        {
            var purchaseOrder = await _repository.GetByIdAsync(PONumber);
            if (purchaseOrder == null)
            {
                return false;
            }

            await _repository.DeleteAsync(purchaseOrder.POnumber);
            return true;
        }

        public async Task<bool> ApprovePurchaseOrderAsync(string PONumber)
        {
            var order = await _repository.GetByIdAsync(PONumber);
            if (order == null || order.State != PurchaseOrderState.Created)
            {
                return false;
            }
            order.Approve();
            await _repository.UpdateAsync(order);
            return true;
        }

        public async Task<bool> ShipPurchaseOrderAsync(string PONumber)
        {
            var order = await _repository.GetByIdAsync(PONumber);
            if (order == null || order.State != PurchaseOrderState.Approved)
            {
                return false;
            }
            order.Ship();
            await _repository.UpdateAsync(order);
            return true;
        }

        public async Task<bool> ClosePurchaseOrderAsync(string PONumber)
        {
            var order = await _repository.GetByIdAsync(PONumber);
            if (order == null || order.State != PurchaseOrderState.Shipped)
            {
                return false;
            }
            order.Close();
            await _repository.UpdateAsync(order);
            return true;
        }





    }
}
