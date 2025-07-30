using System.ComponentModel.DataAnnotations;

namespace ShopProject.Models.Request
{
    public class UpdateProductRequest
    {
        [Required]
        public int? ProductId { get; set; }

        [Required]
        public string? field { get; set; }

        [Required]
        public object? Update {  get; set; }
    }
}
