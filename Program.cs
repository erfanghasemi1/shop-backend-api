using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ShopProject.Middleware;
using ShopProject.Query;
using ShopProject.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Uncomment the following if you're developing a frontend or hosting this API publicly:
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin() // Allow any domain (including localhost:3000, etc.)
//               .AllowAnyHeader()
//               .AllowAnyMethod();
//     });
// });


// configuring JWT Bearer to use it for authentication

var JwtConfig = builder.Configuration.GetSection("Jwt");
var Key = Encoding.UTF8.GetBytes(JwtConfig["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ValidIssuer = JwtConfig["Issuer"],
        ValidAudience = JwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSingleton<AES>();

builder.Services.AddScoped<SignupQuery>();

builder.Services.AddScoped<WalletQuery>();

builder.Services.AddScoped<OrderQuery>();

builder.Services.AddScoped<ProductQuery>();

builder.Services.AddSingleton<JWTGenerator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseRouting();

// Uncomment the following if you're developing a frontend or hosting this API publicly:
// app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<SignupMiddleware>();
app.UseMiddleware<LoginMiddleware>();
app.UseMiddleware<UploadProductMiddleware>();
app.UseMiddleware<RatingProductsMiddleware>();
app.UseMiddleware<OrderProductMiddleware>();
app.UseMiddleware<UpdateProductMiddleware>();

app.Run();
