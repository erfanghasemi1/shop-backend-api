using System.ComponentModel.DataAnnotations;

namespace ShopProject.Models.Request
{
    public class RateProduct
    {
        public int? UserId { get; set; }

        [Required]
        public int? ProductId { get; set; }

        [Required]
        [Range (1,5)]
        public int? stars { get; set; }

        public string? comment { get; set; }
    }
}
