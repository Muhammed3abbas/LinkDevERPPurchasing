using Microsoft.Extensions.Logging;
using Purchasing.Application.DTOs;
using Purchasing.Domain.Interfaces;
using Purchasing.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Application.Services
{
    public class ItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemService> _logger;

        public ItemService(IItemRepository itemRepository, ILogger<ItemService> logger = null)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        public async Task<PurchaseOrderItem> CreateItemAsync(string name, decimal price,int quantity)
        {
            var item = new PurchaseOrderItem(name, price,quantity);
            await _itemRepository.AddAsync(item);
            return item;
        }

        public async Task<PurchaseOrderItem> GetItemByIdAsync(string id)
        {
            var item =  await _itemRepository.GetByIdAsync(id);
            if (item != null)
            {
                return item;
            }
            else
            {
                _logger.LogError($"Item with id {id} not found");
                return null;
            }


        }

        public async Task<IEnumerable<PurchaseOrderItem>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAllAsync();
        }
        public async Task<IEnumerable<PurchaseOrderItemDTO>> GetPagedItemsAsync(int pageNumber, int pageSize, string? nameFilter)
        {
            var items = await _itemRepository.GetPagedItemsAsync(pageNumber, pageSize, nameFilter);
            return items.Select(item => new PurchaseOrderItemDTO
            {
                Code = item.Code,
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity
            });
        }


        public async Task UpdateItemAsync(string code, string? name, decimal? price , int? quantity)
        {
            var item = await _itemRepository.GetByIdAsync(code);
            if (item != null)
            {
                item.Update(name, price,quantity);
                await _itemRepository.UpdateAsync(item);
            }
            else
            {
                _logger.LogError($"Item with id {code} not found");
            }
        }


        public async Task DeleteItemAsync(string code)
        {
            var item = await _itemRepository.GetByIdAsync(code);
            if (item != null)
            {
                await _itemRepository.SoftDeleteAsync(item);
            }
            else
            {
                _logger.LogError($"Item with id {code} not found");
            }
        }
    }
}
