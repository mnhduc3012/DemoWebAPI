
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ProductManagementWebClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";

        public ProductController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7246/api/products";

        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Product> listProducts = JsonSerializer.Deserialize<List<Product>>(strData, options);
            return View(listProducts);
        }

        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{ProductApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Product product = JsonSerializer.Deserialize<Product>(strData, options);
            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7246/api/categories");
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Category> categories = JsonSerializer.Deserialize<List<Category>>(strData, options);

            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(ProductApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to create product. Please try again.");
                }
            }

            HttpResponseMessage responseCategories = await client.GetAsync("https://localhost:7246/api/categories");
            string strData = await responseCategories.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Category> categories = JsonSerializer.Deserialize<List<Category>>(strData, options);

            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Get product by ID
            HttpResponseMessage response = await client.GetAsync($"{ProductApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Product product = JsonSerializer.Deserialize<Product>(strData, options);

            // Get categories
            HttpResponseMessage responseCategories = await client.GetAsync("https://localhost:7246/api/categories");
            string categoriesData = await responseCategories.Content.ReadAsStringAsync();
            List<Category> categories = JsonSerializer.Deserialize<List<Category>>(categoriesData, options);

            
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"{ProductApiUrl}/{product.ProductId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update product. Please try again.");
                }
            }

            HttpResponseMessage responseCategories = await client.GetAsync("https://localhost:7246/api/categories");
            string categoriesData = await responseCategories.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Category> categories = JsonSerializer.Deserialize<List<Category>>(categoriesData, options);

            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View(product);
        }

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			// Retrieve the product details for confirmation
			HttpResponseMessage response = await client.GetAsync($"{ProductApiUrl}/{id}");
			if (response.IsSuccessStatusCode)
			{
				string strData = await response.Content.ReadAsStringAsync();
				var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
				Product product = JsonSerializer.Deserialize<Product>(strData, options);
				return View(product); // Pass the product details to the Delete view
			}

			return RedirectToAction("Index"); // Redirect if the product is not found
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			// Send a DELETE request to the API
			HttpResponseMessage response = await client.DeleteAsync($"{ProductApiUrl}/{id}");
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index"); // Redirect to the Index view on success
			}

			// Handle failure scenario
			ModelState.AddModelError("", "Failed to delete the product. Please try again.");
			return View(); // Optionally, you could return the Delete view with an error message
		}






	}
}
