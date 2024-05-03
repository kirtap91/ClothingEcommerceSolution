using ClothingEcommerceSharedLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClothingEcommerceSharedLibrary.Models
{
    //public class Product
    //{
    //    public int Id { get; set; }
    //    [Required]
    //    public string? Name { get; set; }
    //    [Required]
    //    public string? Description { get; set; }
    //    [Required, Range(0.1, 99999.99)]
    //    public decimal Price { get; set; }
    //    [Required, DisplayName("Product Image")]
    //    public string? Base64Img { get; set; }
    //    [Required, Range(1, 99999)]
    //    public int Quantity { get; set; }
    //    public bool Featured { get; set; } = false;
    //    public DateTime DateUploaded { get; set; } = DateTime.Now;

    //    public string? Category { get; set; }
    //    public string? Brand { get; set; }


    //    public virtual ICollection<ProductVariant> Variants { get; set; } = [];
    //}
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        [Range(0.1, 99999.99)]
        public decimal Price { get; set; }  // Default price if no variants exist
        public int Quantity { get; set; }  // Default quantity if no variants exist
        public bool Featured { get; set; } = false;
        public DateTime DateUploaded { get; set; } = DateTime.Now;

        public Category Category { get; set; }  // Assuming Category is an enum or separate class
        public string? Brand { get; set; }

        public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }

}
