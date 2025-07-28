using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Models.Request;
using ShopProject.Query;

namespace ShopProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly ProductQuery _productQuery;
        private readonly WalletQuery _walletQuery;
        private readonly OrderQuery _orderQuery;

        public OrderController(ProductQuery pq , WalletQuery wq , OrderQuery oq)
        {
            _productQuery = pq;
            _walletQuery = wq;
            _orderQuery = oq;
        }

        [Authorize]
        [HttpPost("order")]
        public async Task<IActionResult> OrderProduct()
        {
            OrderRequest? request = HttpContext.Items["request"] as OrderRequest;

            int stock = await _productQuery.GetProductstockAsycn(request.ProductId);

            if (stock < request.stock) return Conflict(new
            {
                Message = "not enough stock!",
                Stock = stock.ToString()
            });

            decimal ProductPrice = await _productQuery.GetProductPriceAsync(request.ProductId);

            decimal? TotalPrice = ProductPrice * request.stock;

            decimal amount = await _walletQuery.GetAmountAsync(request.UserId);

            if (amount < TotalPrice) return BadRequest(new { Message = "not enough funds in your wallet!" , Wallet = amount.ToString() });

            await _orderQuery.OrderProductAsync(request.stock, request.UserId, request.ProductId, TotalPrice);

            return Ok(new { Messsage = "You ordered the Product successfully."});
        }
    }
}
