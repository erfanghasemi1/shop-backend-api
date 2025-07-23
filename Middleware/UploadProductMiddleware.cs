using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ShopProject.Models.Request;

namespace ShopProject.Middleware
{
    public class UploadProductMiddleware
    {
        private readonly RequestDelegate _next;

        public UploadProductMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/product/upload" , StringComparison.OrdinalIgnoreCase) &&
                context.Request.Method.Equals("POST" , StringComparison.OrdinalIgnoreCase))
            {
                // reading input data (Json file)
                context.Request.EnableBuffering();
                var reader = new StreamReader (context.Request.Body , leaveOpen: true);
                var response = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var request = JsonSerializer.Deserialize<UploadProduct>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // validate data
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
                context.Items["ProductData"] = request;
            }
           await _next(context);
        }
    } 
}
