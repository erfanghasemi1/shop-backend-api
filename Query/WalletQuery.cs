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

        public async Task<decimal> GetAmountAsync(int UserId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"select Wallet from Users where Id = @id";
                try
                {
                    decimal amount = await connection.QueryFirstOrDefaultAsync<decimal>(query, new
                    {
                        id = UserId
                    });
                    return amount;
                }
                catch
                {
                    return -1;
                }
            }
        }
    }
}
