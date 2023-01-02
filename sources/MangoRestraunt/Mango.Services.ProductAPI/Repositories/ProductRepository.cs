using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Dto;
using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int productId);
        Task<ProductDto> CreateUpdateProduct(ProductDto productDto);
        Task<bool> DeleteProduct(int productId);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            var model = _mapper.Map<Product>(productDto);

            try
            {
                if (productDto.ProductId > 0)
                    _dbContext.Products.Update(model);
                else
                    _dbContext.Products.Add(model);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return _mapper.Map<ProductDto>(model);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
                return false;

            _dbContext.Remove(product);
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _dbContext.Products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
