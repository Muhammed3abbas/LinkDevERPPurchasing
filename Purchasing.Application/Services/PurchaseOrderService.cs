using AutoMapper;
using Purchasing.Application.DTOs;
using Purchasing.Domain.DTOs;
using Purchasing.Domain.DTOs.PurchaseOrder;
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
    //public class PurchaseOrderService
    //{
    //    private readonly IPurchaseOrderRepository _repository;

    //    public PurchaseOrderService(IPurchaseOrderRepository repository)
    //    {
    //        _repository = repository;
    //    }

    //    public async Task<PurchaseOrder> CreatePurchaseOrderAsync(List<PurchaseOrderItemDTO> items)
    //    {
    //        var purchaseOrder = new PurchaseOrder();

    //        foreach (var itemDto in items)
    //        {
    //            purchaseOrder.AddItem(itemDto.Code, itemDto.Quantity);
    //        }

    //        await _repository.AddAsync(purchaseOrder);
    //        return purchaseOrder;
    //    }

    //    public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(string PONumber)
    //    {
    //        return await _repository.GetByIdAsync(PONumber);
    //    }

    //    public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
    //    {
    //        return await _repository.GetAllAsync();
    //    }

    //    public async Task<PurchaseOrder> UpdatePurchaseOrderAsync(string PONumber, string orderNumber, DateTime date, decimal totalPrice, List<PurchaseOrderItemDTO> items)
    //    {
    //        var purchaseOrder = await _repository.GetByIdAsync(PONumber);
    //        if (purchaseOrder == null)
    //        {
    //            return null;
    //        }

    //        purchaseOrder.UpdateDetails(orderNumber, date, totalPrice);
    //        purchaseOrder.ClearItems();

    //        foreach (var itemDto in items)
    //        {
    //            purchaseOrder.AddItem(itemDto.Code, itemDto.Quantity);
    //        }

    //        await _repository.UpdateAsync(purchaseOrder);
    //        return purchaseOrder;
    //    }

    //    public async Task<bool> DeletePurchaseOrderAsync(string PONumber)
    //    {
    //        var purchaseOrder = await _repository.GetByIdAsync(PONumber);
    //        if (purchaseOrder == null)
    //        {
    //            return false;
    //        }

    //        await _repository.DeleteAsync(purchaseOrder.POnumber);
    //        return true;
    //    }

    //    public async Task<bool> ApprovePurchaseOrderAsync(string PONumber)
    //    {
    //        var order = await _repository.GetByIdAsync(PONumber);
    //        if (order == null || order.State != PurchaseOrderState.Created)
    //        {
    //            return false;
    //        }
    //        order.Approve();
    //        await _repository.UpdateAsync(order);
    //        return true;
    //    }

    //    public async Task<bool> ShipPurchaseOrderAsync(string PONumber)
    //    {
    //        var order = await _repository.GetByIdAsync(PONumber);
    //        if (order == null || order.State != PurchaseOrderState.Approved)
    //        {
    //            return false;
    //        }
    //        order.Ship();
    //        await _repository.UpdateAsync(order);
    //        return true;
    //    }

    //    public async Task<bool> ClosePurchaseOrderAsync(string PONumber)
    //    {
    //        var order = await _repository.GetByIdAsync(PONumber);
    //        if (order == null || order.State != PurchaseOrderState.Shipped)
    //        {
    //            return false;
    //        }
    //        order.Close();
    //        await _repository.UpdateAsync(order);
    //        return true;
    //    }





    //}

    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private static readonly Random _random = new Random();

        public PurchaseOrderService(IPurchaseOrderRepository repository, IItemRepository itemRepository, IMapper mapper)
        {
            _repository = repository;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<PurchaseOrderReadDTO> CreatePurchaseOrderAsync(List<PurchaseOrderItemBuyDTO> items)
        {
            var purchaseOrder = new PurchaseOrder();

            foreach (var itemDto in items)
            {
                // Get the existing PurchaseOrderItem from the database
                var item = await _itemRepository.GetByIdAsync(itemDto.Code);

                if (item == null)
                {
                    throw new InvalidOperationException($"Item with code {itemDto.Code} does not exist.");
                }

                if (item.Quantity < itemDto.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for item with code {itemDto.Code}. Available: {item.Quantity}, Requested: {itemDto.Quantity}");
                }

                // Check if the item is already part of the purchase order's mappings
                var existingMapping = purchaseOrder.PurchaseOrderItemMappings
                    .FirstOrDefault(mapping => mapping.PurchaseOrderItemCode == itemDto.Code);

                if (existingMapping != null)
                {
                    // If the item exists in the mapping, update the quantity
                    existingMapping.Quantity += itemDto.Quantity;
                }
                else
                {
                    // Create a new mapping for the purchase order and item
                    var newMapping = new PurchaseOrderItemMapping
                    {
                        PurchaseOrder = purchaseOrder,
                        PurchaseOrderItem = item,
                        Quantity = itemDto.Quantity 
                    };

                    purchaseOrder.PurchaseOrderItemMappings.Add(newMapping);

                
                }

                // Adjust stock quantity for the item
                item.Quantity -= itemDto.Quantity;

                // Save the changes to the item
                await _itemRepository.UpdateAsync(item);
            }

            // Recalculate the total price of the purchase order
            //purchaseOrder.TotalPrice = purchaseOrder.Items.Sum(i => i.Price * i.Quantity);
            purchaseOrder.TotalPrice = purchaseOrder.PurchaseOrderItemMappings.Sum(mapping => mapping.PurchaseOrderItem.Price * mapping.Quantity);


            // Save the purchase order with all mappings
            await _repository.AddAsync(purchaseOrder);

            // Map and return the result
            return _mapper.Map<PurchaseOrderReadDTO>(purchaseOrder);
        }

        public async Task<PurchaseOrderReadDTO> GetPurchaseOrderByIdAsync(string PONumber)
        {
            //return await _repository.GetByIdAsync(PONumber);

            var purchaseOrder = await _repository.GetByIdAsync(PONumber);
            if (purchaseOrder == null)
            {
                return null;
            }

            // Map to PurchaseOrderReadDTO using AutoMapper
            return _mapper.Map<PurchaseOrderReadDTO>(purchaseOrder);
        }

        public async Task<List<PurchaseOrderReadDTO>> GetAllPurchaseOrdersAsync()
        {
            var purchaseOrders = await _repository.GetAllAsync();
            return _mapper.Map<List<PurchaseOrderReadDTO>>(purchaseOrders);

        }

        public async Task<PurchaseOrder> UpdatePurchaseOrderAsync(string PONumber, string orderNumber, DateTime date, decimal totalPrice, List<PurchaseOrderItemBuyDTO> items)
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
                var item = await _itemRepository.GetByIdAsync(itemDto.Code);
                if (item == null)
                {
                    throw new InvalidOperationException($"Item with code {itemDto.Code} does not exist.");
                }

                item.AdjustQuantity(-itemDto.Quantity);
                //purchaseOrder.AddItem(item, itemDto.Quantity);
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

            await _repository.DeleteAsync(PONumber);
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

        public async Task<bool> DeactivatePurchaseOrderAsync(string PONumber)
        {
            var order = await _repository.GetByIdAsync(PONumber);
            if (order == null || order.ActivationState != PurchaseOrderActivationState.Activated)
            {
                return false;
            }
            order.Deactivate();
            await _repository.UpdateAsync(order);
            return true;
        }



        private int GenerateCode()
        {
            int randomNumber = _random.Next(1000000, 10000000);
            return randomNumber;
        }
    }



}
