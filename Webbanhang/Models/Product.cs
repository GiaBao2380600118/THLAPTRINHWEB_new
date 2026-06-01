using System.ComponentModel.DataAnnotations;

namespace Webbanhang.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 100000000.00)]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        public List<string>? ImageUrls { get; set; }

        public string? VideoUrl { get; set; }
    }
}
