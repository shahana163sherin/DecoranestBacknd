using AutoMapper;
using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;

namespace DecoranestBacknd.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, AdminProductDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductID))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImgUrl));

            CreateMap<Product, AdminProductDetailDTO>()
                .ForMember(dest=>dest.ProductId,opt=>opt.MapFrom(src=>src.ProductID))
                .ForMember(dest=>dest.CategoryName,opt=>opt.MapFrom(src=>src.Category.CategoryName))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                 .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))

                .ForMember(dest=>dest.ImageUrl,opt=>opt.MapFrom(src=>src.ImgUrl));

            CreateMap<Order, AdminOrderDTO>()
                .ForMember(des => des.OrdeId, opt => opt.MapFrom(src => src.OrderID))
                .ForMember(des => des.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Name : "N/A"))
                .ForMember(des => des.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : "N/A"))
                .ForMember(des => des.PaymentStatus, opt => opt.MapFrom(src => src.Payment != null ? src.Payment.Status : "Not Paid"))
                .ForMember(des => des.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, AdminOrderItemDTO>();
        }
    }
}
