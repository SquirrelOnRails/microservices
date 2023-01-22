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
        private readonly ICouponService _couponService;

        private async Task<string> GetAccessToken()
        {
            return await HttpContext.GetTokenAsync("access_token");
        }
        private string GetUserId()
        {
            return User.Claims.SingleOrDefault(u => u.Type == "sub")?.Value;
        }

        public CartController(IProductService productService, 
            IShoppingCartService shoppingCartService,
            ICouponService couponService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _couponService = couponService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetUserShoppingCart());
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var accessToken = await GetAccessToken();

            var response = await _shoppingCartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);
            if (response != null && response.IsSuccess)
                return RedirectToAction(nameof(Index));
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var accessToken = await GetAccessToken();

            var response = await _shoppingCartService.ApplyCouponAsync<ResponseDto>(cartDto, accessToken);
            if (response != null && response.IsSuccess)
                return RedirectToAction(nameof(Index));
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var accessToken = await GetAccessToken();

            var response = await _shoppingCartService.RemoveCouponAsync<ResponseDto>(cartDto.CartHeader.UserId, accessToken);
            if (response != null && response.IsSuccess)
                return RedirectToAction(nameof(Index));
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            return View(await GetUserShoppingCart());
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
            {
                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var couponResponse = await _couponService.GetCouponAsync<ResponseDto>(cartDto.CartHeader.CouponCode, accessToken);
                    if (couponResponse != null && couponResponse.IsSuccess)
                    {
                        var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(couponResponse.Result));
                        cartDto.CartHeader.DiscountTotal = coupon.DiscountAmount;
                    }
                }

                cartDto.CartHeader.OrderTotal = cartDto.CartDetails.Sum(d => d.Count * d.Product.Price);

                cartDto.CartHeader.OrderTotal -= cartDto.CartHeader.DiscountTotal;
            }
            
            return cartDto;
        }
    }
}
