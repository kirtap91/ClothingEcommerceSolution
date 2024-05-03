using System.ComponentModel.DataAnnotations;

namespace ClothingEcommerceSharedLibrary.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; } 
        [Required]
        public string? Size { get; set; }
        [Required]
        public string? Color { get; set; }
        public int Quantity { get; set; }
        [Range(0.1, 99999.99)]
        public decimal Price { get; set; } 
        public virtual Product? Product { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; } = [];
    }
}

