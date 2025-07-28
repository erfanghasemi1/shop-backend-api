using Dapper;
using MySqlConnector;

namespace ShopProject.Query
{
    public class OrderQuery
    {
        private readonly string? _connectionString;

        public OrderQuery(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("mysqlconnection");
        }

        public async Task OrderProductAsync( int? Stock , int? UserId , int? ProductId , decimal? TotalAmount)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // updating the product's stock
                string ProductStockQuery = @"update Products set Stock = Stock - @stock where Id = @id";

                await connection.ExecuteAsync(ProductStockQuery, new
                {
                    stock = Stock,
                    id = ProductId
                });

                // updating user's wallet
                string UserAmountQuery = @"update Users set Wallet = Wallet - @amount where Id = @id";

                await connection.ExecuteAsync(UserAmountQuery, new
                {
                    amount = TotalAmount,
                    id = UserId
                });

                // inserting the order to the Orders table
                string OrderQuery = @"insert into Orders (BuyerId , TotalAmount) value (@id , @totalAmount); 
                                      select LAST_INSERT_ID()";

                int OrderId = await connection.ExecuteScalarAsync<int>(OrderQuery, new
                {
                    id = UserId,
                    totalAmount = TotalAmount
                });

                // inserting the product that was ordered to the OrderItems
                string OrderItemsQuery = @"insert into OrderItems (OrderId , ProductId , Quantity , Price)
                                           value (@Oid , @Pid , @q , @p)";

                await connection.ExecuteAsync(OrderItemsQuery, new
                {
                    Oid = OrderId,
                    Pid = ProductId,
                    q = Stock,
                    p = TotalAmount / Stock
                });
            }
        }
    }
}
