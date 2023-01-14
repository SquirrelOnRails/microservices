using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartApi.Models
{
    public class CartDetails
    {
        public CartDetails()
        {
            Count = 1;
        }

        [Key]
        public int CartDetailsId { get; set; }

        [Required]
        public int CartHeaderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("CartHeaderId")]
        public virtual CartHeader CartHeader { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
