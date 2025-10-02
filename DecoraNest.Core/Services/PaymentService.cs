using DecoranestBacknd.Configurations;
using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Razorpay.Api;

namespace DecoranestBacknd.DecoraNest.Core.Services
{
    public class PaymentService:IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly RazorpaySettings _settings;

        public PaymentService(ApplicationDbContext context, IOptions<RazorpaySettings> settings)
        {
            _context = context;
            _settings = settings.Value;
        }

        public async Task<Entities.Payment> CreatePaymentAsync(int orderid, decimal amount)
        {
            var client = new RazorpayClient(_settings.Key, _settings.Secret);
            var options = new Dictionary<string, object>
            {
                { "amount",(amount * 100).ToString() },
                {"currency","INR" },
                {"receipt",orderid.ToString() }
            };

            var order = client.Order.Create(options);
            var payment = new Entities.Payment
            {
                OrderId = orderid,
                RazorPayOrderId = order["id"],
                Amount = amount,
                Currency = "INR",
                Status = "created",
                PaymentDate = DateTime.Now
            };
            _context.Add(payment);
            await _context.SaveChangesAsync();
            await _context.Entry(payment).Reference(p => p.Order).LoadAsync();

            return payment;
        }

        public async Task<bool> VerifyPayment(PaymentDTO dto)
        {
            var client = new RazorpayClient(_settings.Key, _settings.Secret);

            Dictionary<string, string> attributes = new Dictionary<string, string>
        {
            { "razorpay_order_id", dto.RazorpayOrderId },
            { "razorpay_payment_id", dto.RazorpayPaymentId },
            { "razorpay_signature", dto.RazorpaySignature }
        };

            try
            {
                Utils.verifyPaymentSignature(attributes); // verifies signature

                var payment = await _context.Payment
                    .FirstOrDefaultAsync(p => p.RazorPayOrderId == dto.RazorpayOrderId);

                if (payment != null)
                {
                    payment.RazorpayPaymentId = dto.RazorpayPaymentId;
                    payment.RazorPaySignature = dto.RazorpaySignature;
                    payment.Status = "paid";

                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }


    }

    
}
