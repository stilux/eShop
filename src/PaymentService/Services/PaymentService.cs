using System;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using PaymentService.Extensions;
using PaymentService.Infrastructure;
using PaymentService.Models.Dtos;

namespace PaymentService.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentContext _context;

        public PaymentService(PaymentContext context)
        {
            _context = context;
        }

        public async Task<string> CreateInvoiceAsync(CreateInvoiceDto model)
        {
            var invoice = model.MapToInvoice();
            
            var faker = new Faker();
            
            invoice.CreationDate = DateTime.Now;
            invoice.ExternalOrderId = faker.Commerce.Ean13();
            invoice.PaymentFormUrl = faker.Internet.Url();

            await _context.AddAsync(invoice);
            await _context.SaveChangesAsync();
            
            return invoice.PaymentFormUrl;
        }

        public async Task CancelPaymentAsync(int orderId)
        {
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.OrderId == orderId);

            if (invoice != null)
            {
                _context.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }
    }
}