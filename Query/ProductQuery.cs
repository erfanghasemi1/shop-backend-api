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

        // retrieve data of a product by its Id

        public async Task<Product?> GetProductByIdQueryAsync(int _id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"select * from Products where Id = @id";

                try
                {
                    Product? product = await connection.QueryFirstOrDefaultAsync<Product?>(query, new { id = _id });
                    return product;
                }
                catch (Exception ex)
                {
                    throw new Exception("getting product by its id from database failed!", ex);
                }
            }
        }

        // retreive data of a product by searching

        public async Task<List<Product>> SearchProductQueryAsync(List<string> Keywords)
        {
            string BaseQuery = "select * from Products where";

            string WhereClause = string.Join(" or ",
                                 Keywords.Select((k, i) => $"(Name like @k{i} or Description like @k{i})"));

            string FinalQuery = BaseQuery + WhereClause + " limit 20";

            var parameters = new DynamicParameters();

            for (int i = 0; i < Keywords.Count; i++)
                parameters.Add($"k{i}", $"%{Keywords[i]}%");

            using (var connection = new MySqlConnection(_connectionString))
            {

                await connection.OpenAsync();

                try
                {
                    IEnumerable<Product> products = await connection.QueryAsync<Product>(FinalQuery,parameters);
                    return products.ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("searching for the product failed!" , ex);
                }
            }
        }

        // inserting the ratings into the database 

        public async Task InsertRatingAsync(RateProduct rateProduct)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                try
                {
                    string query = @"insert into Ratings (ProductId , UserId , Stars , Comment)
                                     value (@pid , @uid , @s , @c)";

                    await connection.ExecuteAsync(query , new
                    {
                        pid = rateProduct.ProductId,
                        uid = rateProduct.UserId , 
                        s = rateProduct.stars ,
                        c = rateProduct.comment 
                    });
                    return;
                }
                catch (Exception ex)
                {
                    throw new Exception("inserting the rate failed!",ex);
                }
            }
        }
    }
}
