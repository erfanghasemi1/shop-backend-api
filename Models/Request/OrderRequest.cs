using System.ComponentModel.DataAnnotations;

namespace ShopProject.Models.Request
{
    public class OrderRequest
    {
        [Required]
        public int? ProductId { get; set; }

        [Required]
        public int? stock {  get; set; }

        public int? UserId { get; set; }
    }
}
