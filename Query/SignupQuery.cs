using Dapper;
using MySqlConnector;
using ShopProject.Models;
using ShopProject.Models.Request;

namespace ShopProject.Query
{
    public class SignupQuery
    {
        private readonly string? _connectionString;

        public SignupQuery(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("mysqlconnection");
        }

        // chekc if the user already sings up
        public async Task<bool> UserExistsAsync(SignupRequest req)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                connection.Execute("use shop");

                string query = @"select Id from Users where Role=@r and (Username=@u or Email=@e)";

                int? id = await connection.QueryFirstOrDefaultAsync<int?>(query, new
                { r = req.Role, u = req.Username, e = req.Email });

                return id.HasValue;
            }
        }
        // add user to the Users table
        public async Task<int> AddUserAsync(User user)
        {
            using(var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                connection.Execute("use shop");

                string query = @"insert into Users (Username , Email , EncryptedPassword , Role , CreatedAt) 
                value(@u,@e,@ep,@r,@ca);
                select LAST_INSERT_ID()";

                int UserId = await connection.ExecuteScalarAsync<int>(query, new 
                {u = user.Username , e = user.Email , ep = user.EncryptedPassword , r = user.Role , ca = user.CreatedAt});
            
                return UserId;
            }
        }
    }
}
