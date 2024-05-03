using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingEcommerceSharedLibrary.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ProductVariantId { get; set; }
        public string? ImageData { get; set; }  // Base64 image data or a link to the image file
        public virtual Product? Product { get; set; }
        public virtual ProductVariant? ProductVariant { get; set; }
    }

}
