namespace Mango.Services.ShoppingCartApi.Models.Dto
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }

        public int CartHeaderId { get; set; }

        public int ProductId { get; set; }

        public virtual CartHeader CartHeader { get; set; }

        public virtual ProductDto Product { get; set; }

        public int Count { get; set; }
    }
}
