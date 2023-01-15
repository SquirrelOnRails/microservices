using Mango.Web.Models;

namespace Mango.Web.Services.Interfaces
{
    public interface IShoppingCartService : IBaseService
    {
        Task<T> GetCartByUserIdAsync<T>(string userId, string token);
        Task<T> AddToCartAsync<T>(CartDto cart, string token);
        Task<T> UpdateCartAsync<T>(CartDto cart, string token);
        Task<T> RemoveFromCartAsync<T>(int cartDetailsId, string token);
        Task<T> ClearCartAsync<T>(int cartHeaderId, string token);
    }
}
