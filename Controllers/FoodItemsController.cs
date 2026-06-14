using CelticsRestaurantAPI.Data;
using CelticsRestaurantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting; 

namespace CelticsRestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FoodItemsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ✅ GET: api/fooditems (Get all food items)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetFoodItems()
        {  //A02: Injection
           var foodItems = await _context.FoodItems.ToListAsync();

            var result = foodItems.Select(item => new
            {
                item.Id,
                item.Name,
                item.Price,
                item.Description,
                item.ImageUrl,
                FullImageUrl = $"http://localhost:5282/images/{item.ImageUrl}" // Ensure FullImageUrl is included
            });

            return Ok(foodItems);
        }
        //A08: Maintain Software and Data Integrity
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFoodImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // A08: Maintain Software & Data Integrity
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(ext))
            {
                return BadRequest("Invalid image format. Only JPG, PNG, or WEBP allowed.");
            }

            // Optional: limit file size (e.g., max 2MB)
            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest("File too large. Max size is 2 MB.");
            }

            // Ensure the "wwwroot/images" directory exists
            var uploadsPath = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // Save image with a unique name to prevent overwriting
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return image filename or URL for frontend to link to
            return Ok(new { message = "Upload successful!", fileName });
        }
    }
}
