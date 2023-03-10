using Mango.Services.ShoppingCartApi.Messages;
using Mango.Services.ShoppingCartApi.Models.Dto;
using Mango.Services.ShoppingCartApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartApi.Controllers
{
    [ApiController]
    [Route("/api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        protected ResponseDto _response;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            _response = new ResponseDto();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserId(userId);
                _response.Result = cartDto;

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }

            return _response;
        }

        [HttpPost("AddToCart")]
        public async Task<object> AddToCart([FromBody] CartDto cartDto)
        {
            try
            {
                var newCart = await _cartRepository.CreateUpdateCart(cartDto);
                _response.Result = newCart;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }

            return _response;
        }

        [HttpPut("UpdateCart")]
        public async Task<object> UpdateCart([FromBody] CartDto cartDto)
        {
            try
            {
                var updatedCart = await _cartRepository.CreateUpdateCart(cartDto);
                _response.Result = updatedCart;

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }

            return _response;
        }

        [HttpDelete("RemoveFromCart/{cartDetailsId}")]
        public async Task<object> RemoveFromCart(int cartDetailsId)
        {
            try
            {
                var removeResult = await _cartRepository.RemoveFromCart(cartDetailsId);
                _response.IsSuccess = removeResult;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }

            return _response;
        }

        [HttpDelete("ClearCart/{cartHeaderId}")]
        public async Task<object> ClearCart(int cartHeaderId)
        {
            try
            {
                var clearResult = await _cartRepository.ClearCart(cartHeaderId);
                _response.IsSuccess = clearResult;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }

            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var couponResult = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
                _response.Result = couponResult;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }

            return _response;
        }

        [HttpDelete("RemoveCoupon/{userId}")]
        public async Task<object> RemoveCoupon(string userId)
        {
            try
            {
                var removeCouponResult = await _cartRepository.RemoveCoupon(userId);
                _response.Result = removeCouponResult;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }

            return _response;
        }
    
        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutHeaderDto checkoutHeader)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserId(checkoutHeader.UserId);
                if (cartDto == null)
                {
                    return BadRequest();
                }
                checkoutHeader.CartDetails = cartDto.CartDetails;
                
                // logic to form message to process order
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }

            return _response;
        }
    }
}
