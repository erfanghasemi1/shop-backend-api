using System.Text.Json;
using ShopProject.Models.Request;
using System.ComponentModel.DataAnnotations;
using ShopProject.Query;
using ShopProject.Utils;

namespace ShopProject.Middleware
{
    public class SignupMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly SignupQuery _query;
        private readonly JWTGenerator _jwtGenerator;


        public SignupMiddleware(RequestDelegate next, IConfiguration configuration , JWTGenerator jWTGenerator)
        {
            _next = next;
            _configuration = configuration;
            _query = new SignupQuery(_configuration);
            _jwtGenerator = jWTGenerator;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            if (context.Request.Path.Equals("/signup", StringComparison.OrdinalIgnoreCase) &&
                (context.Request.Method.Equals("POST",StringComparison.OrdinalIgnoreCase)))
            {
                // reading the request body
                context.Request.EnableBuffering();

                var reader = new StreamReader(context.Request.Body , leaveOpen: true);
                var response = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var request = JsonSerializer.Deserialize<SignupRequest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // validate the request fields 
                var ValidationContext = new ValidationContext(request);
                var results = new List<ValidationResult>();

                bool IsValid = Validator.TryValidateObject(
                    request,
                    ValidationContext,
                    results,
                    true
                );

                if (!IsValid)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Errors = results.Select(e => e.ErrorMessage)
                    });
                    return;
                }

                // check if the user has signed up
                if (await _query.UserExistsAsync(request))
                {
                    context.Response.StatusCode = 409;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Errors = "you already singup . try to login "
                    });
                    return;
                }
                context.Items["SignupRequest"] = request;

                await _next(context);

                string UserId = context.Items["UserId"]?.ToString();

                // debugging 
                Console.WriteLine($"userid : {UserId}");

                var token = _jwtGenerator.GenerateToken(UserId,request.Username,request.Role);

                context.Response.StatusCode = 200;

                await context.Response.WriteAsJsonAsync(new { message = "the user added successfully",Token = token });

            }

            else await _next(context);
        }

    }
}
