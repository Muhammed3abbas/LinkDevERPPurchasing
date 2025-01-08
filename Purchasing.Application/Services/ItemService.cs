using AutoMapper;
using Microsoft.Extensions.Logging;
using Purchasing.Domain.DTOs;
using Purchasing.Domain.DTOs.PurchaseOrderItems;
using Purchasing.Domain.Interfaces;
using Purchasing.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Purchasing.Application.Services
{


    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemService> _logger;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, ILogger<ItemService> logger = null, IMapper mapper = null)
        {
            _itemRepository = itemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PurchaseOrderItemDTO> CreateItemAsync(string name, decimal price, int quantity)
        {
            var item = new PurchaseOrderItem(name, price, quantity);
            await _itemRepository.AddAsync(item);
            return _mapper.Map<PurchaseOrderItemDTO>(item);
        }


        public async Task<PurchaseOrderItem> GetItemByIdAsync(string id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
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



        public async Task<PaginatedResultDTO<PurchaseOrderItemDTO>> GetPagedItemsAsync(int pageNumber, int pageSize, string? nameFilter)
        {
            var (items, totalCount) = await _itemRepository.GetPagedItemsAsync(pageNumber, pageSize, nameFilter);

            return new PaginatedResultDTO<PurchaseOrderItemDTO>
            {
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Items = items.Select(item => _mapper.Map<PurchaseOrderItemDTO>(item)).ToList()
            };
        }
        public async Task<PurchaseOrderItem> UpdateItemAsync(PurchaseOrderItemUpdateDTO itemUpdateDTO)
        {
            var item = await _itemRepository.GetByIdAsync(itemUpdateDTO.Code);
            if (item != null)
            {

                item.Name = string.IsNullOrWhiteSpace(itemUpdateDTO.Name) ? item.Name : itemUpdateDTO.Name;
                item.Price = itemUpdateDTO.Price ?? item.Price;
                item.Quantity = itemUpdateDTO.Quantity ?? item.Quantity;


                await _itemRepository.UpdateAsync(item);
                return item;




            }
            else
            {
                _logger.LogError($"Item with Code {itemUpdateDTO.Code} not found");
                return null;
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
