using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopProject.Data;
using ShopProject.Middleware;
using ShopProject.Query;
using ShopProject.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var ConnectionString = builder.Configuration.GetConnectionString("mysqlconnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});


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

builder.Services.AddSingleton<JWTGenerator>();

builder.Services.AddScoped<ProductQuery>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<SignupMiddleware>();
app.UseMiddleware<LoginMiddleware>();
app.UseMiddleware<UploadProductMiddleware>();
app.UseMiddleware<RatingProductsMiddleware>();
app.UseMiddleware<OrderProductMiddleware>();

app.Run();
