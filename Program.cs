using Microsoft.EntityFrameworkCore;
using ShopProject.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var ConnectionString = builder.Configuration.GetConnectionString("mysqlconnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
