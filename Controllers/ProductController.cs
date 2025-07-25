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

        // retrieve data of a product by its Id

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            Product? product = await productQuery.GetProductByIdQueryAsync(id);

            if (product == null) return NotFound();

            return Ok(product);
        }

        // retrieve data of a product by searching 

        [HttpGet("product")]
        public async Task<IActionResult> SearchProduct(string search)
        {
            List<string> Keywords = search.ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(k => k.Trim())
                .ToList();

            if (Keywords.Count == 0) return NotFound();

            List<Product> RawResults = await productQuery.SearchProductQueryAsync(Keywords);

            var RankedResults = RawResults
                .Select(p => new
                {
                    product = p,

                    score = Keywords.Sum(k =>
                        (p.Name?.ToLower().Contains(k) == true ? 10 : 0) +
                        (p.Description?.ToLower().Contains(k) == true ? 5 : 0)
                    )
                }).OrderByDescending(x => x.score).Select(x => x.product).ToList();

            return Ok(RankedResults);
        }
    }
}
