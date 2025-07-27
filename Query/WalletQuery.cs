using Dapper;
using MySqlConnector;

namespace ShopProject.Query
{
    public class WalletQuery
    {
        private readonly string? _connectionString;

        public WalletQuery(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("mysqlconnection");
        }

        public async Task DepositAsync(int UserId , decimal? amount)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"update Users set Wallet = Wallet + @a where Id = @id";

                try
                {
                    await connection.ExecuteAsync(query, new
                    {
                        a = amount,
                        id = UserId
                    });
                    return;
                }
                catch (Exception ex)
                {
                    throw new Exception("deposit failed!", ex);
                }
            }
        }
    }
}
