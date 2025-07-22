using System.ComponentModel.DataAnnotations;

namespace ShopProject.Models.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage ="username cannot be empty!")]
        public string Username { get; set; }


        [Required(ErrorMessage ="password cannot be empty!")]
        [MinLength(8,ErrorMessage ="Password is not correct!")]
        public string Password { get; set; }


        [Required(ErrorMessage ="Role cannot be empty!")]
        [RegularExpression("^(Seller|Customer)$",ErrorMessage ="Role must be either Seller or Customer!")]
        public string Role { get; set; }

    }
}
