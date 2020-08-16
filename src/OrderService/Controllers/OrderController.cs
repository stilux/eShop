using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Services;
using OrderService.Exceptions;
using OrderService.Models.Dtos;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order != null)
                return Ok(order);
            return NotFound();
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<OrderDto>> CreateOrder()
        {
            var order = await _orderService.CreateOrderAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
        
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<OrderDto>> CancelOrder(int id)
        {
            try
            {
                await _orderService.CancelOrderAsync(id);
                return Ok();
            }
            catch (OrderNotFoundException)
            {
                return NotFound();
            }
            catch (CanceledOrderOperationException)
            {
                return BadRequest("Order already canceled.");
            }
            catch (Exception)
            {
                return UnprocessableEntity();
            }
        }
        
        [HttpPost("{id}/pay")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<string>> PayOrder(int id)
        {
            try
            {
                var paymentUrl = await _orderService.PayOrderAsync(id);
                return Ok(paymentUrl);
            }
            catch (OrderNotFoundException)
            {
                return NotFound();
            }
            catch (CanceledOrderOperationException)
            {
                return BadRequest("Order already canceled.");
            }
            catch (OrderAlreadyPaidException)
            {
                return BadRequest("Order already paid.");
            }
            catch (Exception)
            {
                return UnprocessableEntity();
            }
        }
        
        [HttpPost("{id}/order-items")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderItemDto>> AddToCart([FromRoute] int id, [FromBody] AddToCartDto model)
        {
            try
            {
                var orderItems = await _orderService.AddToCartAsync(id, model);
                return Created(string.Empty, orderItems);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpDelete("{id}/order-items")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> RemoveFromCart([FromRoute] int id, [FromBody] RemoveFromCartDto model)
        {
            await _orderService.RemoveFromCartAsync(id, model);
            return NoContent();
        }
        
        [HttpPut("{orderId}/order-items/{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<OrderDto>> ChangeOrderItemQuantity([FromRoute] int orderId, [FromRoute] int productId, 
            [FromBody] ChangeOrderItemQuantityDto model)
        {
            try
            {
                await _orderService.ChangeOrderItemQuantityAsync(orderId, productId, model);
                return NoContent();
            }
            catch (OrderItemNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return UnprocessableEntity();
            }
        }
    }
}