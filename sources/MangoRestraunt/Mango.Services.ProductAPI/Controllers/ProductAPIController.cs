using Mango.Services.ProductAPI.Dto;
using Mango.Services.ProductAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("/api/products")]
    public class ProductAPIController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductAPIController(IProductRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            var response = new ResponseDto();

            try
            {
                var products = await _repository.GetProducts();
                response.Result = products.ToList();
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { e.ToString() };
            }

            return response;
        }

        [Authorize]
        [HttpGet("{productId}")]
        public async Task<ResponseDto> Get(int productId)
        {
            var response = new ResponseDto();

            try
            {
                var product = await _repository.GetProductById(productId);
                response.Result = product;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { e.ToString() };
            }

            return response;
        }

        [Authorize]
        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] ProductDto productDto)
        {
            var response = new ResponseDto();

            try
            {
                var product = await _repository.CreateUpdateProduct(productDto);
                response.Result = product;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { e.ToString() };
            }

            return response;
        }

        [Authorize]
        [HttpPut]
        public async Task<ResponseDto> Put([FromBody] ProductDto productDto)
        {
            var response = new ResponseDto();

            try
            {
                var product = await _repository.CreateUpdateProduct(productDto);
                if (product == null)
                    response.IsSuccess = false;
                else
                    response.Result = product;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { e.ToString() };
            }

            return response;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productId}")]
        public async Task<ResponseDto> Delete(int productId)
        {
            var response = new ResponseDto();

            try
            {
                var deleteResult = await _repository.DeleteProduct(productId);
                response.Result = deleteResult;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { e.ToString() };
            }

            return response;
        }
    }
}
