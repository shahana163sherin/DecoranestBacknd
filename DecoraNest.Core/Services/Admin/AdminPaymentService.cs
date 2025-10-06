using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Ecommerce.Shared.Responses;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace DecoranestBacknd.DecoraNest.Core.Services.Admin
{
    public class AdminPaymentService:IAdminPaymentService
    {
        private readonly ApplicationDbContext _context;
        public AdminPaymentService(ApplicationDbContext context)
        {
            _context = context;
        }
        private AdminPaymentDTO MapToDTO(Payment p) => new AdminPaymentDTO
        {
            PaymentId = p.PaymentId,
            OrderId = p.OrderId,
            Username = p.Order.User?.Name,
            Email = p.Order.User?.Email,
            Amount = p.Amount,
            Status = p.Status,
            PaymentDate = p.PaymentDate
        };
        public async Task<PagedResult<AdminPaymentDTO>> GetAllPaymentAsync(int pagenumber, int limit)
        {
            var totalPayments = await _context.Payment.CountAsync();
            var payments=await _context.Payment
                .Include(p=>p.Order)
                .ThenInclude(o=>o.User)
                .OrderBy(p=>p.PaymentId)
                .Skip((pagenumber - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var totalPages=(int)Math.Ceiling((double)totalPayments/limit);
            return new PagedResult<AdminPaymentDTO>
            {
                Items = payments.Select(MapToDTO).ToList(),
                CurrentPage = pagenumber,
                PageSize = limit,
                TotalItems = totalPayments,
                TotalPages = totalPages
            };

        }

        public async Task<ApiResponse<AdminPaymentDTO?>> GetPaymentByIdAsync(int id)
        {
            var payments=await _context.Payment
                .Include(p=>p.Order)
                .ThenInclude(o=>o.User)
                .FirstOrDefaultAsync(p=>p.PaymentId==id);
            if (payments == null)
            {
              return new ApiResponse<AdminPaymentDTO?>
                {
                    Status = "error",
                    Message = "Payment not found",
                    Data = null
                };
            }
            return new ApiResponse<AdminPaymentDTO?>
            {
                Status = "success",
                Message = "Payment found",
                Data = MapToDTO(payments)
            };
        }
            public async Task<ApiResponse<IEnumerable<AdminPaymentDTO>>> SearchByName(string username)
            {
                var search=await _context.Payment
                .Include(p=>p.Order)
                .ThenInclude(o=>o.User)
                .Where(p=>p.Order.User.Name.ToLower().Contains(username.ToLower()))
                .ToListAsync();

            if (search == null || !search.Any())

            {
                return new ApiResponse<IEnumerable<AdminPaymentDTO>>
                {
                    Status = "Error",
                    Message = "No payment found",
                    Data = null
                };

            }
            return new ApiResponse<IEnumerable<AdminPaymentDTO>>
            {
                Status = "Success",
                Message = "Payment found",
                Data = search.Select(o => MapToDTO(o)).ToList()
            };


        }
        public async Task<ApiResponse<IEnumerable<AdminPaymentDTO>>> GetByStatus(string status)
        {
            var result = await _context.Payment
                .Include(p => p.Order)
                .ThenInclude(o => o.User)
                .Where(p => p.Status.ToLower() == status.ToLower())
                .ToListAsync();
            if(result == null || !result.Any())
            {
                return new ApiResponse<IEnumerable<AdminPaymentDTO>>
                {
                    Status = "Error",
                    Message = "No payments found",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<AdminPaymentDTO>>
            {
                Status = "Success",
                Message = "Payments found",
                Data = result.Select(MapToDTO).ToList()
            };
        }
        public async Task<ApiResponse<IEnumerable<AdminPaymentDTO>>> SortByDate(bool ascending)
        {
            var payment = _context.Payment
                .Include(p => p.Order)
                .ThenInclude(o => o.User);
              
            var sort = ascending
                ? await payment.OrderBy(o => o.PaymentDate).ToListAsync() 
                : await payment.OrderByDescending(o => o.PaymentDate).ToListAsync();
            var result = sort.Select(MapToDTO).ToList();
         return new ApiResponse<IEnumerable<AdminPaymentDTO>>
            {
                Status = "Success",
                Message = ascending ? "Newest payments first" : "Oldest payments first",
                Data = result
            };
        }
        public async Task<ApiResponse<bool>> UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var payment=await _context.Payment.FirstOrDefaultAsync(p=>p.PaymentId==paymentId);
            if(payment == null)
            {
                return new ApiResponse<bool>
                {
                    Status = "Error",
                    Message = "Payment not found",
                    Data = false
                };
            }

            payment.Status = status;
            _context.Payment.Update(payment);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool>
            {
                Status = "Success",
                Message = "Payment status updated",
                Data = true
            };
        }
        public async Task<ApiResponse<bool>> DeletePaymentAsync(int paymentId)
        {
            var payment = await _context.Payment.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            if(payment == null)
            {
               return new ApiResponse<bool>
                {
                    Status = "Error",
                    Message = "Payment not found",
                    Data = false
                };
            }
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool>
            {
                Status = "Success",
                Message = "Payment deleted successfully",
                Data = true
            };
        }

    }
}
