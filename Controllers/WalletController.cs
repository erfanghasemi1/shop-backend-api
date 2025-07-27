using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Query;

namespace ShopProject.Controllers
{
    public class WalletController : Controller
    {
        private readonly WalletQuery walletQuery;

        public WalletController(WalletQuery wq)
        {
            walletQuery = wq;
        }


        [Authorize]
        [HttpPost("wallet/deposit")]
        public async Task<IActionResult> DepositWallet([FromBody] decimal? value)
        {
            if (value == null || value <= 0) return BadRequest();

            string? UserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (!int.TryParse(UserIdClaim, out var userId)) return BadRequest();

            await walletQuery.DepositAsync(userId, value);

            return Ok("deposit is done successfully.");
        }
    }
}
