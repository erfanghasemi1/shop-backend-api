using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using ShopProject.Models.Request;

namespace ShopProject.Middleware
{
    public class RatingProductsMiddleware
    {
        private readonly RequestDelegate _next;

        public RatingProductsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/product/rate", StringComparison.OrdinalIgnoreCase) &&
               context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                // reading input data from Json file
                context.Request.EnableBuffering();

                StreamReader reader = new StreamReader(context.Request.Body , leaveOpen: true);
                var response = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                RateProduct? request = JsonSerializer.Deserialize<RateProduct>(response , new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // validating the input data 
                if (request == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Errors = "request is empty!"
                    });
                    return;
                }
                ValidationContext validation = new ValidationContext(request);
                List<ValidationResult> results = new List<ValidationResult>();

                bool IsValid = Validator.TryValidateObject(
                    request,
                    validation,
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

                context.Items["data"] = request;
            }

            await _next(context);
        }
    }
}
