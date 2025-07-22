using Microsoft.EntityFrameworkCore;
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

builder.Services.AddSingleton<AES>();

builder.Services.AddScoped<SignupQuery>();

builder.Services.AddSingleton<JWTGenerator>();

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

app.Run();
