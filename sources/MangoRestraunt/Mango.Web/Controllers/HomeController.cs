using Mango.Web.Models;
using Mango.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _cartService;

        private async Task<string> GetAccessToken()
        { 
            return await HttpContext.GetTokenAsync("access_token");
        }
        private string GetUserId()
        {
            return User.Claims.SingleOrDefault(u => u.Type == "sub")?.Value;
        }
        
        public HomeController(ILogger<HomeController> logger, 
            IProductService productService, 
            IShoppingCartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = new();

            var productsResponse = await _productService.GetAllProductsAsync<ResponseDto>("");
            if (productsResponse != null && productsResponse.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(productsResponse.Result));
            }

            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            ProductDto product = new();

            var productsResponse = await _productService.GetProductByIdAsync<ResponseDto>(productId, "");
            if (productsResponse != null && productsResponse.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(productsResponse.Result));
            }

            return View(product);
        }

        [Authorize]
        [HttpPost]
        [ActionName("Details")]
        public async Task<IActionResult> AddDetailsToCart(ProductDto productDto)
        {
            var token = await GetAccessToken();

            CartDto cartDto = new() 
            { 
                CartHeader = new CartHeaderDto 
                { 
                    UserId = GetUserId()
                }
            };

            var cartDetails = new CartDetailsDto
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };

            var productResponse = await _productService.GetProductByIdAsync<ResponseDto>(productDto.ProductId, token);
            if (productResponse != null && productResponse.IsSuccess)
            {
                cartDetails.Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(productResponse.Result));
            }

            cartDto.CartDetails = new List<CartDetailsDto> { cartDetails };

            var addToCartResponse = await _cartService.AddToCartAsync<ResponseDto>(cartDto, token);
            if (addToCartResponse != null && addToCartResponse.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            // for debug purposes
            //var access = await HttpContext.GetTokenAsync("access_token");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}