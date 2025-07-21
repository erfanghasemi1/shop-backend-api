using Microsoft.AspNetCore.Mvc;
using ShopProject.Models;
using ShopProject.Models.Request;
using ShopProject.Query;
using ShopProject.Utils;

namespace ShopProject.Controllers
{
    public class SignupController : Controller
    {
        private readonly AES _aes;
        private readonly SignupQuery _signupQuery;

        public SignupController(AES aes , SignupQuery signupQuery)
        {
            _aes = aes;
            _signupQuery = signupQuery;
        }

        [HttpPost("signup")]
        public async Task Singup()
        {
            var request = HttpContext.Items["SignupRequest"] as SignupRequest;
            User user = new User
            {
                Username = request.Username,
                Email = request.Email,
                EncryptedPassword = _aes.Encrypt(request.Password),
                Role = request.Role,
                CreatedAt = DateTime.Now
            };

            int UserId = await _signupQuery.AddUserAsync(user);

            HttpContext.Items["UserId"] = UserId;

        }
    }
}
