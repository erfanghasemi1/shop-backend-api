using Dapper;
using MySqlConnector;
using ShopProject.Models.Request;

namespace ShopProject.Query
{
    public class LoginQuery
    {
        private readonly string? _connectionString;

        public LoginQuery(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("mysqlconnection");
        }

        public async Task<LoginQueryData?> LoginAsync(LoginRequest req)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"select Id,EncryptedPassword from Users where Username=@u and Role=@r limit 1";

                LoginQueryData? LQD = await connection.QueryFirstOrDefaultAsync<LoginQueryData?>(query, new
                {
                    u = req.Username,
                    r = req.Role
                });

                return LQD;
            }
        }
    }
}
