using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchasing.Application.Services;
using global::Purchasing.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Purchasing.Domain.DTOs;


namespace Purchasing.API.Controllers
{


    //[ApiController]
    //[Route("api/[controller]")]
    //public class PurchaseOrderController : ControllerBase
    //{
    //    private readonly PurchaseOrderService _service;

    //    public PurchaseOrderController(PurchaseOrderService service)
    //    {
    //        _service = service;
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> CreatePurchaseOrder([FromBody] PurchaseOrderDTO purchaseOrderDto)
    //    {
    //        if (purchaseOrderDto == null)
    //        {
    //            return BadRequest("PurchaseOrder data is required.");
    //        }

    //        var purchaseOrder = await _service.CreatePurchaseOrderAsync(

    //            purchaseOrderDto.Items);

    //        return CreatedAtAction(nameof(GetPurchaseOrderById), new { id = purchaseOrder.POnumber }, purchaseOrder);
    //    }

    //    [HttpGet("{id}")]
    //    public async Task<IActionResult> GetPurchaseOrderById(string id)
    //    {
    //        var purchaseOrder = await _service.GetPurchaseOrderByIdAsync(id);
    //        if (purchaseOrder == null)
    //        {
    //            return NotFound();
    //        }
    //        return Ok(purchaseOrder);
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> GetAllPurchaseOrders()
    //    {
    //        var purchaseOrders = await _service.GetAllPurchaseOrdersAsync();
    //        return Ok(purchaseOrders);
    //    }

    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> UpdatePurchaseOrder(string id, [FromBody] PurchaseOrderDTO purchaseOrderDto)
    //    {
    //        if (purchaseOrderDto == null)
    //        {
    //            return BadRequest("PurchaseOrder data is required.");
    //        }

    //        var updatedPurchaseOrder = await _service.UpdatePurchaseOrderAsync(
    //            id,
    //            purchaseOrderDto.OrderNumber,
    //            purchaseOrderDto.Date,
    //            purchaseOrderDto.TotalPrice,
    //            purchaseOrderDto.Items);

    //        if (updatedPurchaseOrder == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(updatedPurchaseOrder);
    //    }

    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeletePurchaseOrder(string id)
    //    {
    //        var result = await _service.DeletePurchaseOrderAsync(id);
    //        if (!result)
    //        {
    //            return NotFound();
    //        }

    //        return NoContent();
    //    }

    //    [HttpPut("{id}/approve")]
    //    public async Task<IActionResult> ApprovePurchaseOrder(string id)
    //    {
    //        var result = await _service.ApprovePurchaseOrderAsync(id);
    //        if (!result)
    //        {
    //            return BadRequest("Only created orders can be approved.");
    //        }
    //        return NoContent();
    //    }

    //    [HttpPut("{id}/ship")]
    //    public async Task<IActionResult> ShipPurchaseOrder(string id)
    //    {
    //        var result = await _service.ShipPurchaseOrderAsync(id);
    //        if (!result)
    //        {
    //            return BadRequest("Only approved orders can be shipped.");
    //        }
    //        return NoContent();
    //    }

    //    [HttpPut("{id}/close")]
    //    public async Task<IActionResult> ClosePurchaseOrder(string id)
    //    {
    //        var result = await _service.ClosePurchaseOrderAsync(id);
    //        if (!result)
    //        {
    //            return BadRequest("Only shipped orders can be closed.");
    //        }
    //        return NoContent();
    //    }



    //}

    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;

        public PurchaseOrderController(IPurchaseOrderService service)
        {
            _service = service;
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

        [HttpGet]
        public async Task<IActionResult> GetAllPurchaseOrders()
        {
            var purchaseOrders = await _service.GetAllPurchaseOrdersAsync();
            return Ok(purchaseOrders);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseOrder(string id, [FromBody] PurchaseOrderDTO purchaseOrderDto)
        {
            if (purchaseOrderDto == null)
            {
                return BadRequest("PurchaseOrder data is required.");
            }

            try
            {
                var updatedPurchaseOrder = await _service.UpdatePurchaseOrderAsync(id, purchaseOrderDto.OrderNumber, purchaseOrderDto.Date,
                    purchaseOrderDto.TotalPrice, purchaseOrderDto.Items);

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


