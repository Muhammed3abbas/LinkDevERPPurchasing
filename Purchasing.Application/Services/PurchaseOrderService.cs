using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Purchasing.Application.DTOs;
using Purchasing.Domain.DTOs;
using Purchasing.Domain.DTOs.PurchaseOrder;
using Purchasing.Domain.DTOs.PurchaseOrderItems;
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
        private readonly IMemoryCache _cache;

        private static readonly Random _random = new Random();

        public PurchaseOrderService(IPurchaseOrderRepository repository, IItemRepository itemRepository, IMapper mapper, IMemoryCache cache)
        {
            _repository = repository;
            _itemRepository = itemRepository;
            _mapper = mapper;
            _cache = cache;
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
        public async Task<(List<PurchaseOrderReadDTO>, int)> GetCachedPurchaseOrdersAsync()
        {
            const string cacheKey = "PurchaseOrdersCache3";
            List<PurchaseOrderReadDTO> cachedOrders;
            int totalCount;

            if (!_cache.TryGetValue(cacheKey, out cachedOrders))
            {
                var purchaseOrders = await _repository.GetAllAsync();

                // Map and take the top 7 for caching
                cachedOrders = _mapper.Map<List<PurchaseOrderReadDTO>>(purchaseOrders.Take(7).ToList());

                // Cache the top 7 purchase orders
                _cache.Set(cacheKey, cachedOrders, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Adjust CacheDuration as needed
                });

                // Get the total count from the database
                totalCount = purchaseOrders.Count();
            }
            else
            {
                // If cached, still need to fetch the total count separately
                totalCount = await _repository.GetTotalCountAsync();
            }

            return (cachedOrders, totalCount);
        }
        public async Task<PaginatedResultDTO<PurchaseOrderReadDTO>> GetPagedPurchaseOrdersAsync(int pageNumber, int pageSize, string? POnumberFilter)
        {
            var (items, totalCount) = await _repository.GetPagedItemsAsync(pageNumber, pageSize, POnumberFilter);

            return new PaginatedResultDTO<PurchaseOrderReadDTO>
            {
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Items = items.Select(item => _mapper.Map<PurchaseOrderReadDTO>(item)).ToList()
            };
        }
        public async Task<PaginatedResultDTO<PurchaseOrderReadDTO>> GetPurchaseOrdersCachedPaginationAsync(int pageNumber, int pageSize)
        {
            const string CacheKey = "Top7PurchaseOrders";
            List<PurchaseOrderReadDTO> cachedOrders;

            if (pageNumber == 1 && _cache.TryGetValue(CacheKey, out cachedOrders))
            {
                return new PaginatedResultDTO<PurchaseOrderReadDTO>
                {
                    Items = cachedOrders,
                    TotalCount = await _repository.GetTotalCountAsync(),
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
            }

            var purchaseOrders = await _repository.GetPagedAsync(pageNumber, pageSize);
            var purchaseOrderDtos = _mapper.Map<List<PurchaseOrderReadDTO>>(purchaseOrders);

            if (pageNumber == 1)
            {
                cachedOrders = purchaseOrderDtos.Take(7).ToList();
                _cache.Set(CacheKey, cachedOrders, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }

            return new PaginatedResultDTO<PurchaseOrderReadDTO>
            {
                Items = purchaseOrderDtos,
                TotalCount = await _repository.GetTotalCountAsync(),
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        //public async Task<PurchaseOrder> UpdatePurchaseOrderAsync(string PONumber, List<PurchaseOrderItemBuyDTO> items)
        //{
        //    var purchaseOrder = await _repository.GetByIdAsync(PONumber);
        //    if (purchaseOrder == null)
        //    {
        //        return null;
        //    }

        //    //purchaseOrder.UpdateDetails(orderNumber, totalPrice);
        //    //purchaseOrder.ClearItems();

        //    foreach (var itemDto in items)
        //    {
        //        var item = await _itemRepository.GetByIdAsync(itemDto.Code);
        //        if (item == null)
        //        {
        //            throw new InvalidOperationException($"Item with code {itemDto.Code} does not exist.");
        //        }

        //        item.AdjustQuantity(-itemDto.Quantity);
        //        //purchaseOrder.AddItem(item, itemDto.Quantity);
        //    }

        //    await _repository.UpdateAsync(purchaseOrder);
        //    return purchaseOrder;
        //}

        public async Task<PurchaseOrder> UpdatePurchaseOrderAsync1(string poNumber, List<PurchaseOrderItemBuyDTO> items)
        {
            var purchaseOrder = await _repository.GetByIdAsync(poNumber);
            if (purchaseOrder == null)
            {
                return null;
            }

            //purchaseOrder.UpdateItems(items, _itemRepository);
            await _repository.UpdateAsync(purchaseOrder);
            return purchaseOrder;
        }


        public async Task<PurchaseOrderReadDTO> UpdatePurchaseOrderAsync(string poNumber, List<PurchaseOrderItemBuyDTO> items)
        {
            // Fetch the existing purchase order
            var purchaseOrder = await _repository.GetByIdAsync(poNumber);
            if (purchaseOrder == null)
            {
                return null;
            }

            // Reset the total price
            decimal newTotalPrice = 0;

            // Keep track of the codes of items that need to be removed
            var existingItemCodes = purchaseOrder.PurchaseOrderItemMappings.Select(m => m.PurchaseOrderItem.Code).ToList();

            foreach (var itemDto in items)
            {
                var item = await _itemRepository.GetByIdAsync(itemDto.Code);
                if (item == null)
                {
                    throw new InvalidOperationException($"Item with code {itemDto.Code} does not exist.");
                }

                if (item.Quantity < itemDto.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient quantity for item {itemDto.Code}.");
                }

                // Adjust stock quantity for the item
                item.Quantity -= itemDto.Quantity;

                // Check if the item already exists in the PurchaseOrder via mappings
                var existingMapping = purchaseOrder.PurchaseOrderItemMappings
                    .FirstOrDefault(mapping => mapping.PurchaseOrderItemCode == itemDto.Code);

                if (existingMapping != null)
                {
                    // adding quantity that will be deducted lately
                    item.Quantity += existingMapping.Quantity;
                    // Update the existing mapping with the new quantity
                    existingMapping.Quantity = itemDto.Quantity;
                    // Remove the code from the list of items to be deleted
                    existingItemCodes.Remove(itemDto.Code);
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

                // Update the total price
                newTotalPrice += item.Price * itemDto.Quantity;

                // Save the updated item to the repository
                await _itemRepository.UpdateAsync(item);
            }

            // Remove any mappings for items that were not included in the updated list
            foreach (var itemCode in existingItemCodes)
            {
                var mappingToRemove = purchaseOrder.PurchaseOrderItemMappings
                    .FirstOrDefault(mapping => mapping.PurchaseOrderItemCode == itemCode);

                if (mappingToRemove != null)
                {
                    purchaseOrder.PurchaseOrderItemMappings.Remove(mappingToRemove);
                }
            }

            // Update the total price of the purchase order
            purchaseOrder.TotalPrice = newTotalPrice;

            // Save the updated purchase order
            await _repository.UpdateAsync(purchaseOrder);

            return _mapper.Map<PurchaseOrderReadDTO>(purchaseOrder);

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

    }



}
