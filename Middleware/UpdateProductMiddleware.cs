using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ShopProject.Models.Request;

namespace ShopProject.Middleware
{
    public class UpdateProductMiddleware
    {
        private readonly RequestDelegate _next;

        public UpdateProductMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/product/update" , StringComparison.OrdinalIgnoreCase) && 
                context.Request.Method.Equals("POST" , StringComparison.OrdinalIgnoreCase)) 
            {
                // reading input data from Json file
                context.Request.EnableBuffering();
                StreamReader reader = new StreamReader(context.Request.Body , leaveOpen: true);
                var response = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                UpdateProductRequest? request = JsonSerializer.Deserialize<UpdateProductRequest?>(response , new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (request == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new { Message = "the Json file is corrupted!" });
                    return;
                }
                
                // validating data 

                ValidationContext validationContext = new ValidationContext(request);
                List<ValidationResult> results = new List<ValidationResult>();

                bool IsValid = Validator.TryValidateObject(
                    request,
                    validationContext,
                    results,
                    true
                );

                if (!IsValid)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new { Message = "missing data!" });
                    return;
                }

                context.Items["request"] = request;
            }

            await _next(context);
        }
    }
}
