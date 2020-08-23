using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentService.Models;
using PaymentService.Models.Dtos;

namespace PaymentService.Services
{
    public interface IPaymentService
    {
        Task<string> CreateInvoiceAsync(CreateInvoiceDto model);
        Task CancelPaymentAsync(int orderId);
    }
}