using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Models.Dtos;
using DeliveryService.Services;

namespace DeliveryService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet("methods")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> Get([FromQuery] string address, [FromQuery] DateTime? deliveryDate)
        {
            if (address == null)
                return BadRequest($"{nameof(address)} is empty.");
            
            if (!deliveryDate.HasValue)
                return BadRequest($"{nameof(deliveryDate)} is empty.");
            
            var deliveryMethods = await _deliveryService.GetDeliveryMethodsAsync(address, deliveryDate.Value);
            return Ok(deliveryMethods);
        }
        
        [HttpPost("requests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<DeliveryRequestDto>> CreateDeliveryRequest(CreateDeliveryRequestDto model)
        {
            var deliveryRequest = await _deliveryService.CreateDeliveryRequestAsync(model);
            if (deliveryRequest != null)
                return Created(string.Empty, deliveryRequest);
            return UnprocessableEntity();
        }
    }
}