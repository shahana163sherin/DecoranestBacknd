using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Infrastructure.Data;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(int orderid,decimal amount);
        Task<bool> VerifyPayment(PaymentDTO dto);
    }
}
