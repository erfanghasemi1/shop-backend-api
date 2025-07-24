using Dapper;
using MySqlConnector;
using ShopProject.Models;
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

        //  add product to the database by Seller
        public async Task AddProductAsync(UploadProduct Data)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

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

        //  retrieve products for the home page 
        public async Task<List<Product>> GetHomepageProductAsync()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"select * from Products limit 10";

                try
                {
                    IEnumerable<Product> results = await connection.QueryAsync<Product>(query);
                    return results.ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("retrieving Products from database failed!", ex);
                }
            }
        }
    }
}
