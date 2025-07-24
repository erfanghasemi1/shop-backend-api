using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Models;
using ShopProject.Models.Request;
using ShopProject.Query;

namespace ShopProject.Controllers
{
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProductQuery productQuery;
        public ProductController(ProductQuery pq)
        {
            productQuery = pq;
        }

        //  Upload Product to database by the Seller

        [Authorize(Roles ="Seller")]
        [HttpPost("product/upload")]
        public async Task<IActionResult> UploadProduct()
        {
            var Data = HttpContext.Items["ProductData"] as UploadProduct;

            await productQuery.AddProductAsync(Data);

            return Ok(new {message = "Product added successfuly"});
        }

        // retrieve Products from database for the home page 

        [HttpGet("home")]
        public async Task<IActionResult> HomepageProduct()
        {
            List<Product> products = await productQuery.GetHomepageProductAsync();   

            return Ok(products);
        }
    }
}
