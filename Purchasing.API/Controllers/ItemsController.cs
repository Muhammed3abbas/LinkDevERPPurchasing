using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchasing.Application.Services;
using Purchasing.Domain.DTOs.PurchaseOrderItems;
using Purchasing.Domain.Interfaces;
using Purchasing.Domain.Models;

namespace Purchasing.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemsController(IItemService itemService )
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
            var count = items.Count();
            return Ok(new { items, count });
        }

        [HttpPost("paged")]
        public async Task<IActionResult> GetPagedItems([FromBody] PurchaseOrderItemPagination? Paging)
        {
            Paging ??= new PurchaseOrderItemPagination{pageNumber = 1,pageSize = 10,nameFilter = null};
            var result = await _itemService.GetPagedItemsAsync((int)Paging.pageNumber, (int)Paging.pageSize, Paging.nameFilter);
            return Ok(result);
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

