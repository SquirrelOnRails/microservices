using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models
{
    [Comment("A product to sell on the website")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string? Name { get; set; }

        public double Price { get; set; }

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
