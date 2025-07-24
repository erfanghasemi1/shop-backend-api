using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ShopProject.Models.Request;
using ShopProject.Query;
using ShopProject.Utils;

namespace ShopProject.Middleware
{
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LoginQuery _query;
        private readonly AES _aes;
        private readonly JWTGenerator _jwtGenerator;

        public LoginMiddleware(RequestDelegate next , IConfiguration configuration,AES aes,JWTGenerator jWTGenerator)
        {
            _next = next;
            _query = new LoginQuery(configuration);
            _aes = aes;
            _jwtGenerator = jWTGenerator;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/login" , StringComparison.OrdinalIgnoreCase) &&
                context.Request.Method.Equals("POST" , StringComparison.OrdinalIgnoreCase))
            {
                // reading the request body ( Json file )
                context.Request.EnableBuffering();

                StreamReader reader = new StreamReader (context.Request.Body , leaveOpen: true);
                var response = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var request = JsonSerializer.Deserialize<LoginRequest>(response , new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // validating data 
                var ValidationContext = new ValidationContext(request);
                var result = new List<ValidationResult>();

                bool IsValid = Validator.TryValidateObject(
                    request,
                    ValidationContext,
                    result,
                    true
                );

                if (!IsValid)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Errors = result.Select(e => e.ErrorMessage)
                    });
                    return;
                }

                // check the user's password 
                LoginQueryData? LQD = await _query.LoginAsync(request);
                string? EncryptedPassword = LQD?.EncryptedPassword;

                // check EncryptedPassword ( if it's null then calling _aes.Decrypt(EncryptedPassword) will make error!
                if (string.IsNullOrEmpty(EncryptedPassword) )
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Errors = "Invalid Username or Password!"
                    });
                    return;
                }

                string DecryptedPassword = _aes.Decrypt(EncryptedPassword);

                if (DecryptedPassword != request.Password)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Errors = "Invalid Username or Password!"
                    });
                    return;
                }

                // give a JWT to the authenticated user 
                string? UserId = LQD?.Id?.ToString() ;

                var token = _jwtGenerator.GenerateToken(UserId, request.Username, request.Role);

                context.Response.StatusCode = 200;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "welcomeeeee.",
                    Token = token
                });

            }
            await _next(context);
        }
    }
}
