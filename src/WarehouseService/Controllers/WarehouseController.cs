using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseService.Exceptions;
using WarehouseService.Models;
using WarehouseService.Services;

namespace WarehouseService.Controllers
{
    [ApiController]
    [Route("/api/v1/warehouse")]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet("products/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetBalance(int id)
        {
            var warehouseItem = await _warehouseService.GetWarehouseItemAsync(id);
            if (warehouseItem != null)
                return Ok(warehouseItem.Balance);
            return NotFound();
        }
        
        [HttpPost("reserves")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<int>> Reserve(ReserveModel model)
        {
            try
            {
                var reserveId = await _warehouseService.ReserveAsync(model);
                return Created(string.Empty, reserveId);
            }
            catch (ProductNotFoundException)
            {
                return BadRequest("Product not found.");
            }
            catch (InsufficientProductQuantityException)
            {
                return BadRequest("Not enough quantity of product.");
            }
            catch (Exception)
            {
                return UnprocessableEntity("An error occurred while reserving items.");
            }
        }
        
        [HttpDelete("reserves/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> CancelReservation(int id)
        {
            try
            {
                await _warehouseService.CancelReservationAsync(id);
                return Ok();
            }
            catch (ReserveNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return UnprocessableEntity("An error occurred when cancel the reservation of goods.");
            }
        }
    }
}