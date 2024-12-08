using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace ProjectManagementAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private ProductRepository repository = new ProductRepository();

		[HttpGet]
		public IActionResult GetProducts() { 
			return Ok(repository.GetProducts());
		}

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = repository.GetProductById(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }

        [HttpPost]
		public IActionResult PostProduct(Product p)
		{
			repository.SaveProduct(p);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteProduct(int id)
		{
			var p = repository.GetProductById(id);
			if (p == null)
			{
				return NotFound();
			}
			repository.DeleteProduct(p);
			return NoContent();
		}

		[HttpPut("{id}")]
		public IActionResult UpdateProduct(int id, Product p)
		{
			var pTmp = repository.GetProductById(id);
			if (p == null)
			{
				return NotFound();
			}
			repository.UpdateProduct(p);
			return NoContent();
		}

	}
}
