using BagAPI.Helper;
using BagAPI.Models;
using BagAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BagAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productService.GetAllAsync();
            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));

            var response = await _productService.GetByIdAsync(id);
            if (response.Success)
                return Ok(response);

            return NotFound(response);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Products product)
        {
            if (product == null)
                return BadRequest(new ApiResponse<string>(false, "Invalid product data.", null));

            var response = await _productService.CreateAsync(product);
            if (response.Success)
                return CreatedAtAction(nameof(GetById), new { id = response.Data.id }, response);

            return BadRequest(response);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Products updatedProduct)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));

            var response = await _productService.UpdateAsync(id, updatedProduct);
            if (response.Success)
                return Ok(response);

            return NotFound(response);
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));

            var response = await _productService.DeleteAsync(id);
            if (response.Success)
                return Ok(response);

            return NotFound(response);
        }
    }
}
