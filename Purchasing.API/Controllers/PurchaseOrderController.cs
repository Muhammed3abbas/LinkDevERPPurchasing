using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchasing.Application.Services;
using global::Purchasing.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Purchasing.Domain.DTOs.PurchaseOrder;
using Purchasing.Domain.DTOs.PurchaseOrderItems;
using Microsoft.Extensions.Caching.Memory;
using Purchasing.Domain.Models;


namespace Purchasing.API.Controllers
{



    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "Top7PurchaseOrders";
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public PurchaseOrderController(IPurchaseOrderService service , IMemoryCache cache)
        {
            _service = service;
            _cache = cache;

        }



        [HttpPost]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] List<PurchaseOrderItemBuyDTO> items)
        {
            if (items == null)
            {
                return BadRequest("items for PurchaseOrder data is required.");
            }

            try
            {
                var purchaseOrder = await _service.CreatePurchaseOrderAsync(items);
                return Ok(purchaseOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseOrderById(string id)
        {
            var purchaseOrderDto = await _service.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrderDto == null)
            {
                return NotFound();
            }

            return Ok(purchaseOrderDto);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllPurchaseOrders()
        //{
        //    var purchaseOrders = await _service.GetAllPurchaseOrdersAsync();
        //    return Ok(purchaseOrders);
        //}


        //[HttpGet]
        //public async Task<IActionResult> GetAllPurchaseOrders()
        //{
        //    var orders = await _service.GetAllPurchaseOrdersAsync();

        //    return Ok(new
        //    {
        //        TotalCount = orders.Count,
        //        Orders = orders
        //    });
        //}

        [HttpPost("Paged-Filteration-PON")]
        public async Task<IActionResult> GetPagedPurchaseOrders([FromBody] PurchaseOrderPagination pagination)
        {
            var result = await _service.GetPagedPurchaseOrdersAsync(
                pagination.PageNumber,
                pagination.PageSize,
                pagination.NameFilter);

            return Ok(result);
        }



        [HttpGet("cached-paginated")]
        public async Task<IActionResult> GetAllPurchaseOrdersCachedPagination(int pageNumber = 1, int pageSize = 7)
        {
            // Call the service to get the paginated purchase orders
            var paginatedResult = await _service.GetPurchaseOrdersCachedPaginationAsync(pageNumber, pageSize);

            // Return the result as an HTTP response
            return Ok(paginatedResult);
        }




        //[HttpPut]
        //public async Task<IActionResult> UpdatePurchaseOrder( [FromBody] PurchaseOrderUpdateDTO purchaseOrderDto)
        //{
        //    if (purchaseOrderDto == null)
        //    {
        //        return BadRequest("PurchaseOrder data is required.");
        //    }

        //    try
        //    {
        //        var updatedPurchaseOrder = await _service.UpdatePurchaseOrderAsync(purchaseOrderDto.POnumber,
        //             purchaseOrderDto.Items);

        //        if (updatedPurchaseOrder == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(updatedPurchaseOrder);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPut]
        public async Task<IActionResult> UpdatePurchaseOrder([FromBody] PurchaseOrderUpdateDTO purchaseOrderDto)
        {
            if (purchaseOrderDto == null)
            {
                return BadRequest("Purchase order data is required.");
            }

            try
            {
                var updatedPurchaseOrder = await _service.UpdatePurchaseOrderAsync(purchaseOrderDto.POnumber, purchaseOrderDto.Items);

                if (updatedPurchaseOrder == null)
                {
                    return NotFound();
                }

                return Ok(updatedPurchaseOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(string id)
        {
            var result = await _service.DeletePurchaseOrderAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApprovePurchaseOrder(string id)
        {
            var result = await _service.ApprovePurchaseOrderAsync(id);
            if (!result)
            {
                return BadRequest("Only created orders can be approved.");
            }
            return Ok("Approved Successfully");
        }

        [HttpPut("{id}/ship")]
        public async Task<IActionResult> ShipPurchaseOrder(string id)
        {
            var result = await _service.ShipPurchaseOrderAsync(id);
            if (!result)
            {
                return BadRequest("Only approved orders can be shipped.");
            }
            return Ok("Order is in Shipping state");
        }

        [HttpPut("{id}/close")]
        public async Task<IActionResult> ClosePurchaseOrder(string id)
        {
            var result = await _service.ClosePurchaseOrderAsync(id);
            if (!result)
            {
                return BadRequest("Only shipped orders can be closed.");
            }
            return Ok("Closed Successfully");
        }

        [HttpPut("{id}/Deactivate")]
        public async Task<IActionResult> DeActivatePurchaseOrder(string id)
        {
            var result = await _service.DeactivatePurchaseOrderAsync(id);
            if (!result)
            {
                return BadRequest("The Order Already Deactivated Before");
            }
            return Ok("Order Has Been Deactivated");
        }

    }


}


