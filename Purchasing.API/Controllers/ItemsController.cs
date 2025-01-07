using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchasing.Application.Services;
using Purchasing.Domain.DTOs;
using Purchasing.Domain.Models;

namespace Purchasing.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseOrderItemCreateDTO itemDto)
        {
            var item = await _itemService.CreateItemAsync(itemDto.Name, itemDto.Price,itemDto.Quantity);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpPost("paged")]
        public async Task<IActionResult> GetPagedItems([FromBody] PurchaseOrderItemPagination Paging)
        {
            var items = await _itemService.GetPagedItemsAsync(Paging.pageNumber, Paging.pageSize, Paging.nameFilter);
            return Ok(items);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PurchaseOrderItemUpdateDTO itemDto)
        {
            var itemUpdated = await _itemService.UpdateItemAsync(itemDto);
            return Ok(itemUpdated);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _itemService.DeleteItemAsync(id);
            return Ok();
        }


    }
}

