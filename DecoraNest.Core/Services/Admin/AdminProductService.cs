using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Ecommerce.Shared.Responses;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DecoranestBacknd.DecoraNest.Core.Services.Admin
{
    public class AdminProductService: IAdminProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;
        public AdminProductService(ApplicationDbContext context, Cloudinary cloudinary,IMapper mapper)
        {
            _context = context;
            _cloudinary = cloudinary;
            _mapper = mapper;
        }
        //private AdminProductDTO MapToDTO(Product p)=> new AdminProductDTO
        //{
        //  ProductId=p.ProductID,
        //  ProductName=p.ProductName,
        //  CategoryName=p.Category.CategoryName,
        //  Price=p.Price,
        //   ImageUrl=p.ImgUrl
        //};
        //private AdminProductDetailDTO MapToDetailDTO(Product p) => new AdminProductDetailDTO
        //{
        //    ProductId = p.ProductID,
        //    ProductName = p.ProductName,
        //    Description = p.Description,
        //    CategoryId = p.CategoryId,
        //    CategoryName = p.Category?.CategoryName,
        //    Price = p.Price,
        //    ImageUrl=p.ImgUrl
        //};

        private async Task<string> UploadImageToCloudinary(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "product-images" 
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl.AbsoluteUri; 
        }

        //public async Task<PagedResult<AdminProductDTO>> GetAllProductsAsync(int pagenumber, int limit)
        //{
        //    var totalProducts = await _context.Products.CountAsync();
        //    var products = _context.Products
        //          .Include(p => p.Category)
        //          .OrderBy(p => p.ProductID)
        //          .Skip((pagenumber - 1) * limit)
        //          .Take(limit)
        //          .ProjectTo<AdminProductDTO>(_mapper.ConfigurationProvider)
        //          //.Select(p => new AdminProductDTO
        //          //{
        //          //    ProductId = p.ProductID,
        //          //    ProductName = p.ProductName,
        //          //    CategoryName = p.Category.CategoryName,
        //          //    ImageUrl = p.ImgUrl,
        //          //    Price = p.Price
        //          //})
        //          .ToListAsync();
        //    //var products=_mapper.Map<List<AdminProductDTO>>(products);
        //    var totalPages = (int)Math.Ceiling((double)totalProducts / limit);
        //    return new PagedResult<AdminProductDTO>
        //    {
        //        Items = products,
        //        CurrentPage = pagenumber,
        //        PageSize = limit,
        //        TotalItems = totalProducts,
        //        TotalPages = totalPages
        //    };


        //}


        public async Task<PagedResult<AdminProductDTO>> GetAllProductsAsync(int pagenumber, int limit)
        {
            var totalProducts = await _context.Products.CountAsync();

            var products = await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.ProductID)
                .Skip((pagenumber - 1) * limit)
                .Take(limit)
                .ProjectTo<AdminProductDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(); // <-- await here

            var totalPages = (int)Math.Ceiling((double)totalProducts / limit);

            return new PagedResult<AdminProductDTO>
            {
                Items = products, // now it's List<AdminProductDTO>
                CurrentPage = pagenumber,
                PageSize = limit,
                TotalItems = totalProducts,
                TotalPages = totalPages
            };
        }





        //public async Task<ApiResponse<AdminProductDTO>> AddProductAsync(ProductUpdateCreateDTO dto)
        //{
        //    string imageUrl = await UploadImageToCloudinary(dto.ImageFile);

        //    var product = new Product
        //    {
        //        ProductName = dto.ProductName,
        //        CategoryId = dto.CategoryId,
        //        Price = dto.Price,
        //        Description = dto.Description,
        //        ImgUrl = imageUrl
        //    };

        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    var createdProduct = await _context.Products
        //        .Include(p => p.Category)
        //        .Where(p => p.ProductID == product.ProductID)
        //        //.Select(p => new AdminProductDTO
        //        //{
        //        //    ProductId = p.ProductID,
        //        //    ProductName = p.ProductName,
        //        //    CategoryName = p.Category.CategoryName,
        //        //    ImageUrl = p.ImgUrl,
        //        //    Price = p.Price
        //        //})
        //        .FirstOrDefaultAsync();
        //    var productDto = _mapper.Map<AdminProductDTO>(createdProduct);

        //    return new ApiResponse<AdminProductDTO>
        //    {
        //        Status = "Success",
        //        Message = "Product added successfully.",
        //        Data = productDto
        //    };
        //}

        public async Task<ApiResponse<AdminProductDTO>> AddProductAsync(ProductUpdateCreateDTO dto)
        {
            // Upload image if provided
            string imageUrl = null;
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                imageUrl = await UploadImageToCloudinary(dto.ImageFile);
            }

            var product = new Product
            {
                ProductName = dto.ProductName,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Description = dto.Description,
                ImgUrl = imageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Fetch product including related data
            var createdProduct = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

            if (createdProduct == null)
            {
                return new ApiResponse<AdminProductDTO>
                {
                    Status = "Error",
                    Message = "Failed to add product",
                    Data = null
                };
            }

            var productDto = _mapper.Map<AdminProductDTO>(createdProduct);

            return new ApiResponse<AdminProductDTO>
            {
                Status = "Success",
                Message = "Product added successfully.",
                Data = productDto
            };
        }

        //public async Task<ApiResponse<AdminProductDTO?>> GetProductByIdAsync(int id)
        //{
        //    var product= await _context.Products
        //        .Include(p=>p.Category)
        //        .Where(p=>p.ProductID==id)
        //        //.Select(p=>new AdminProductDTO
        //        //{
        //        //    ProductId = p.ProductID,
        //        //    ProductName = p.ProductName,
        //        //    CategoryName = p.Category.CategoryName,
        //        //    ImageUrl = p.ImgUrl,
        //        //    Price = p.Price
        //        //})
        //        .FirstOrDefaultAsync();
        //    var productDto = _mapper.Map<AdminProductDTO?>(product);
        //    if (product == null)
        //       {
        //            return new ApiResponse<AdminProductDTO?>
        //            {
        //                Status = "Error",
        //                Message = "Product not found",
        //                Data = null
        //            };
        //        }
        //    return new ApiResponse<AdminProductDTO?>
        //    {
        //        Status = "Success",
        //        Message = "Product found",
        //        Data = productDto
        //    };
        //}

        public async Task<ApiResponse<AdminProductDTO?>> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
            {
                return new ApiResponse<AdminProductDTO?>
                {
                    Status = "Error",
                    Message = "Product not found",
                    Data = null
                };
            }

            var productDto = _mapper.Map<AdminProductDTO>(product);

            return new ApiResponse<AdminProductDTO?>
            {
                Status = "Success",
                Message = "Product found",
                Data = productDto
            };
        }




        //public async Task<ApiResponse<AdminProductDTO?>> UpdateProductAsync(int id, ProductUpdateCreateDTO dto)
        //{
        //    var product = await _context.Products.FindAsync(id);

        //    if (product == null)
        //    {
        //        return new ApiResponse<AdminProductDTO?>
        //        {
        //            Status = "Error",
        //            Message = $"Product with ID {id} not found.",
        //            Data = null
        //        };
        //    }

        //    product.ProductName = dto.ProductName;
        //    product.Description = dto.Description;
        //    product.CategoryId = dto.CategoryId;
        //    product.Price = dto.Price;

        //    if (dto.ImageFile != null && dto.ImageFile.Length > 0)
        //    {
        //        product.ImgUrl = await UploadImageToCloudinary(dto.ImageFile);
        //    }

        //    await _context.SaveChangesAsync();

        //    var updatedProduct = await _context.Products
        //        .Include(p => p.Category)
        //        .Where(p => p.ProductID == product.ProductID)
        //        //.Select(p => new AdminProductDTO
        //        //{
        //        //    ProductId = p.ProductID,
        //        //    ProductName = p.ProductName,
        //        //    CategoryName = p.Category.CategoryName,
        //        //    ImageUrl = p.ImgUrl,
        //        //    Price = p.Price
        //        //})
        //        .FirstOrDefaultAsync();
        //    var productDto = _mapper.Map<AdminProductDTO?>(updatedProduct);

        //    return new ApiResponse<AdminProductDTO?>
        //    {
        //        Status = "Success",
        //        Message = "Product updated successfully.",
        //        Data = productDto
        //    };
        //}


        public async Task<ApiResponse<AdminProductDTO?>> UpdateProductAsync(int id, ProductUpdateCreateDTO dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return new ApiResponse<AdminProductDTO?>
                {
                    Status = "Error",
                    Message = $"Product with ID {id} not found.",
                    Data = null
                };
            }

            // Update fields
            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;
            product.Price = dto.Price;

            // Upload image if provided
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                product.ImgUrl = await UploadImageToCloudinary(dto.ImageFile);
            }

            await _context.SaveChangesAsync();

            // Retrieve updated product with category details
            var updatedProduct = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

            if (updatedProduct == null) // Safety check
            {
                return new ApiResponse<AdminProductDTO?>
                {
                    Status = "Error",
                    Message = "Error retrieving updated product",
                    Data = null
                };
            }

            var productDto = _mapper.Map<AdminProductDTO>(updatedProduct);

            return new ApiResponse<AdminProductDTO?>
            {
                Status = "Success",
                Message = "Product updated successfully.",
                Data = productDto
            };
        }



        public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return new ApiResponse<bool>
                {
                    Status = "Error",
                    Message = "Product not found",
                    Data = false
                };

            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool>
            {
                Status = "Success",
                Message = "Product deleted successfully",
                Data = true
            };
        }
        //public async Task<ApiResponse<AdminProductDTO?>> SearchByName(string name)
        //{
        //    var product= await _context.Products
        //        .Include(p => p.Category)
        //        .Where(p => p.ProductName.ToLower().Contains(name.ToLower()))
        //        //.Select(p => new AdminProductDTO
        //        //{
        //        //    ProductId = p.ProductID,
        //        //    ProductName = p.ProductName,
        //        //    CategoryName = p.Category.CategoryName,
        //        //    ImageUrl = p.ImgUrl,
        //        //    Price = p.Price
        //        //})
        //        .FirstOrDefaultAsync();
        //    var productDto = _mapper.Map<AdminProductDTO?>(product);
        //    if (product == null)
        //    {
        //        return new ApiResponse<AdminProductDTO?>
        //        {
        //            Status = "Error",
        //            Message = "Product not found",
        //            Data = null
        //        };
        //    }

        //    return new ApiResponse<AdminProductDTO?>
        //    {
        //        Status = "Success",
        //        Message = "Product found",
        //        Data = productDto
        //    };
        //}


        public async Task<ApiResponse<AdminProductDTO?>> SearchByName(string name)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductName.ToLower().Contains(name.ToLower()));

            if (product == null)
            {
                return new ApiResponse<AdminProductDTO?>
                {
                    Status = "Error",
                    Message = "Product not found",
                    Data = null
                };
            }

            var productDto = _mapper.Map<AdminProductDTO>(product);

            return new ApiResponse<AdminProductDTO?>
            {
                Status = "Success",
                Message = "Product found",
                Data = productDto
            };
        }

        //public async Task<ApiResponse<IEnumerable<AdminProductDTO>>>GetByCategory(string categoryName)
        //{
        //    var result = await _context.Products
        //        .Include(p => p.Category)
        //        .Where(p => p.Category.CategoryName.ToLower() == categoryName.ToLower())
        //        .ToListAsync();

        //    if(result == null)
        //    {
        //        return new ApiResponse<IEnumerable<AdminProductDTO>>
        //        {
        //            Status = "Error",
        //            Message = "No products found for the specified category",
        //            Data = null
        //        };

        //    }
        //    var products = result.Select(p => new AdminProductDTO
        //    {
        //        ProductId = p.ProductID,
        //        ProductName = p.ProductName,
        //        CategoryName = p.Category.CategoryName,
        //        ImageUrl = p.ImgUrl,
        //        Price = p.Price
        //    });

        //    return new ApiResponse<IEnumerable<AdminProductDTO>>
        //    {
        //        Status = "Success",
        //        Message = "Products retrieved successfully",
        //        Data = products
        //    };
        //}
        public async Task<ApiResponse<IEnumerable<AdminProductDTO>>> GetByCategory(string categoryName)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Category.CategoryName.ToLower() == categoryName.ToLower())
                .ToListAsync();

            if (products == null || !products.Any())
            {
                return new ApiResponse<IEnumerable<AdminProductDTO>>
                {
                    Status = "Error",
                    Message = "No products found for the specified category",
                    Data = null
                };
            }

            var productDtos = _mapper.Map<IEnumerable<AdminProductDTO>>(products);

            return new ApiResponse<IEnumerable<AdminProductDTO>>
            {
                Status = "Success",
                Message = "Products retrieved successfully",
                Data = productDtos
            };
        }


    }
}
