using Mango.Web.Models;
using Mango.Web.Services.Interfaces;

namespace Mango.Web.Services
{
    public class ShoppingCartService : BaseService, IShoppingCartService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ShoppingCartService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> AddToCartAsync<T>(CartDto cart, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                Url = new Uri(new Uri(SD.ShoppingCartAPIBase), "/api/cart/AddToCart").ToString(),
                AccessToken = token,
                Method = SD.ApiType.POST,
                Data = cart
            });
        }

        public async Task<T> ClearCartAsync<T>(int cartHeaderId, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                Url = new Uri(new Uri(SD.ShoppingCartAPIBase), "/api/cart/ClearCart/{cartHeaderId}").ToString(),
                AccessToken = token,
                Method = SD.ApiType.DELETE
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                Url = new Uri(new Uri(SD.ShoppingCartAPIBase), "/api/cart/GetCart/{userId}").ToString(),
                AccessToken = token,
                Method = SD.ApiType.GET
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartDetailsId, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                Url = new Uri(new Uri(SD.ShoppingCartAPIBase), "/api/cart/RemoveFromCart/{cartDetailsId}").ToString(),
                AccessToken = token,
                Method = SD.ApiType.DELETE
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cart, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                Url = new Uri(new Uri(SD.ShoppingCartAPIBase), "/api/cart/UpdateCart").ToString(),
                AccessToken = token,
                Method = SD.ApiType.PUT,
                Data = cart
            });
        }
    }
}
