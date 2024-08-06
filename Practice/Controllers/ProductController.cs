using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models; 

namespace Practice.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPatch("{id}/price")]
        public async Task<ActionResult<Product>> UpdateProductPrice(int id, [FromBody] decimal price)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Price = price;
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpGet("sorted")]
        public async Task<ActionResult<IEnumerable<Product>>> GetSortedProducts([FromQuery] string sortOrder)
        {
            IQueryable<Product> productsQuery = _context.Products;

            switch (sortOrder?.ToLower())
            {
                case "asc":
                    productsQuery = productsQuery.OrderBy(p => p.Name);
                    break;
                case "desc":
                    productsQuery = productsQuery.OrderByDescending(p => p.Name);
                    break;
                default:
                    return BadRequest();
            }

            return await productsQuery.ToListAsync();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteProducts([FromBody] IEnumerable<int> ids)
        {
            var products = await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
            if (products == null || !products.Any())
            {
                return NotFound();
            }

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("description/{keyword}")]
        public async Task<ActionResult<Product>> GetProductByDescriptionKeyword(string keyword)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Description.Contains(keyword));
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        
    }
}
