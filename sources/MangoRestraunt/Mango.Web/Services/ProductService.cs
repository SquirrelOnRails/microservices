using Mango.Web.Models;
using Mango.Web.Services.Interfaces;

namespace Mango.Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> CreateProductAsync<T>(ProductDto productDto)
        {
            return await this.SendAsync<T>(new ApiRequest 
            {
                Url = $"{SD.ProductAPIBase}/api/products",
                Method = SD.ApiType.POST,
                AccessToken = "",
                Data = productDto
            });
        }

        public async Task<T> DeleteProductAsync<T>(int productId)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                Url = $"{SD.ProductAPIBase}/api/products/{productId}",
                Method = SD.ApiType.DELETE,
                AccessToken = ""
            });
        }

        public async Task<T> GetAllProductsAsync<T>()
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                Url = $"{SD.ProductAPIBase}/api/products",
                Method = SD.ApiType.GET,
                AccessToken = ""
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int productId)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                Url = $"{SD.ProductAPIBase}/api/products/{productId}",
                Method = SD.ApiType.GET,
                AccessToken = ""
            });
        }

        public async Task<T> UpdateProductAsync<T>(ProductDto productDto)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                Url = $"{SD.ProductAPIBase}/api/products",
                Method = SD.ApiType.PUT,
                AccessToken = "",
                Data = productDto
            });
        }
    }
}
