using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ProductRepository repository = new ProductRepository();


        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(repository.GetCategories());
        }
    }
}
