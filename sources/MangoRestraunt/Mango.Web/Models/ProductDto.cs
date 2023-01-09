using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class ProductDto
    {
        public ProductDto()
        {
            Count = 1;
        }

        public int ProductId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        public string? ImageUrl { get; set; }

        [Range(1, 100)]
        public int Count { get; set; }
    }
}
