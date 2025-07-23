using System.ComponentModel.DataAnnotations;

namespace ShopProject.Models.Request
{
    public class UploadProduct
    {
        [Required]
        public int? SellerId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public int? Stock { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }
    }
}
