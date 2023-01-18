using Mango.Web.Models;
using Mango.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;

        private async Task<string> GetAccessToken()
        {
            return await HttpContext.GetTokenAsync("access_token");
        }
        private string GetUserId()
        {
            return User.Claims.SingleOrDefault(u => u.Type == "sub")?.Value;
        }

        public CartController(IProductService productService, IShoppingCartService shoppingCartService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetUserShoppingCart());
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var accessToken = await GetAccessToken();

            var response = await _shoppingCartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);
            if (response == null || !response.IsSuccess || response.Result == null)
                return RedirectToAction(nameof(Index));
            else
                return View();
        }

        private async Task<CartDto> GetUserShoppingCart()
        {
            var userId = GetUserId();
            var accessToken = await GetAccessToken();

            var response = await _shoppingCartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);
            if (response == null || !response.IsSuccess || response.Result == null)
                return null;
            
            var cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            
            if (cartDto.CartHeader != null)
                cartDto.CartHeader.OrderTotal = cartDto.CartDetails.Sum(d => d.Count * d.Product.Price);
            
            return cartDto;
        }
    }
}
