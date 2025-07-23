using Dapper;
using MySqlConnector;
using ShopProject.Models.Request;

namespace ShopProject.Query
{
    public class ProductQuery
    {
        private readonly string? _connectionString;
        public ProductQuery(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("mysqlconnection");
        }

        public async Task AddProduct(UploadProduct Data)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"insert into Products (SellerId , Name , Description , Price , Stock , CreatedAt)
                                value (@si , @n , @d , @p , @s , @c)";
                try
                {
                    await connection.ExecuteAsync(query, new
                    {
                        si = Data.SellerId,
                        n = Data.Name,
                        d = Data.Description,
                        p = Data.Price,
                        s = Data.Stock,
                        c = Data.CreatedAt
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception("insert the product failed!",ex);
                }
            }
        }
    }
}
