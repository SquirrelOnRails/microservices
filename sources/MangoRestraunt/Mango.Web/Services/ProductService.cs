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

        public async Task<T> CreateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest 
            {
                Url = new Uri(new Uri(SD.ProductAPIBase), "api/products").ToString(),
                Method = SD.ApiType.POST,
                AccessToken = token,
                Data = productDto
            });
        }

        public async Task<T> DeleteProductAsync<T>(int productId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                Url = new Uri(new Uri(SD.ProductAPIBase), $"api/products/{productId}").ToString(),
                Method = SD.ApiType.DELETE,
                AccessToken = token
            });
        }

        public async Task<T> GetAllProductsAsync<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                Url = new Uri(new Uri(SD.ProductAPIBase), "api/products").ToString(),
                Method = SD.ApiType.GET,
                AccessToken = token
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int productId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                Url = new Uri(new Uri(SD.ProductAPIBase), $"api/products/{productId}").ToString(),
                Method = SD.ApiType.GET,
                AccessToken = token
            });
        }

        public async Task<T> UpdateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                Url = new Uri(new Uri(SD.ProductAPIBase), "api/products").ToString(),
                Method = SD.ApiType.PUT,
                AccessToken = token,
                Data = productDto
            });
        }
    }
}
