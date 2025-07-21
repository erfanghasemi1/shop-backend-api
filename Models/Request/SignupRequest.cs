using System.ComponentModel.DataAnnotations;

namespace ShopProject.Models.Request
{
    public class SignupRequest
    {
        [Required(ErrorMessage ="username cannot be empty!")]
        public string Username { get; set; }


        [Required(ErrorMessage ="password cannot be empty!")]
        [MinLength(8,ErrorMessage ="password's length must be more than 7 characters!")]
        public string Password { get; set; }


        [Required(ErrorMessage ="Email cannot be empty!")]
        [EmailAddress(ErrorMessage ="Email format is invalid!")]
        public string Email { get; set; }


        [Required(ErrorMessage ="role cannot be empty!")]
        [RegularExpression("^(Seller|Buyer)$",ErrorMessage ="role must be either Seller or Buyer!")]
        public string Role { get; set; }

    }
}
