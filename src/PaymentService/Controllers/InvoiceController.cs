using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Models.Dtos;
using PaymentService.Services;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/v1/invoices")]
    public class InvoiceController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public InvoiceController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<string>> CreateInvoice(CreateInvoiceDto model)
        {
            if (model.CartItems.Count == 0)
                return BadRequest();
            
            try
            {
                var paymentFormUrl = await _paymentService.CreateInvoiceAsync(model);
                return Created(string.Empty, paymentFormUrl);
            }
            catch (Exception)
            {
                return UnprocessableEntity();
            }
        }
    }
}