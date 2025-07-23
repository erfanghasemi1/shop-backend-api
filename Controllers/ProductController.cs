using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Models.Request;
using ShopProject.Query;

namespace ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductQuery productQuery;
        public ProductController(ProductQuery pq)
        {
            productQuery = pq;
        }
        [Authorize(Roles ="Seller")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadProduct()
        {
            var Data = HttpContext.Items["ProductData"] as UploadProduct;

            await productQuery.AddProduct(Data);

            return Ok(new {message = "Product added successfuly"});
        }
    }
}
