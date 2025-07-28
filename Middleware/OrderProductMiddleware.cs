using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;
using ShopProject.Models.Request;

namespace ShopProject.Middleware
{
    public class OrderProductMiddleware
    {
        private readonly RequestDelegate _next;

        public OrderProductMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/order", StringComparison.OrdinalIgnoreCase) &&
                context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                // reading request data 

                context.Request.EnableBuffering();

                StreamReader reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var response = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var request = JsonSerializer.Deserialize<OrderRequest>(response , new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (request == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync("the request data is missed!");
                    return;
                }

                string? UserIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(UserIdClaim, out var UserId))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync("there is problem with the JWT!");
                    return;
                }

                request.UserId = UserId;

                // validate the request data 
                ValidationContext validationContext = new ValidationContext(request);
                List<ValidationResult> result = new List<ValidationResult>();

                bool IsValid = Validator.TryValidateObject(
                    request,
                    validationContext,
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

                context.Items["request"] = request;
            }

            await _next(context);
        }
    }
}
