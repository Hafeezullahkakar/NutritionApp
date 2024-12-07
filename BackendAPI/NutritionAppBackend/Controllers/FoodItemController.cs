using Microsoft.AspNetCore.Mvc;
using NutritionAppBackend.Data;
using NutritionAppBackend.Models;
using NutritionAppBackend.Services;

namespace NutritionAppBackend.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class FoodItemController : Controller
    {

        private readonly NutritionDbContext _nutritionDbContext;
        private readonly FoodItemService _foodItemServier;

        public FoodItemController(NutritionDbContext nutritionDbContext, FoodItemService foodItemServic)
        {
            _nutritionDbContext = nutritionDbContext;
            _foodItemServier = foodItemServic;
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddFoodItem([FromBody] FoodItem item)
        {
            if (item == null)
            {
                return BadRequest("Please send Item");
            }

            var itemAdded = await _foodItemServier.AddFoodItem(item); // Ensure this is awaited

            if (itemAdded != null)
            {
                return Ok(itemAdded);
            }
            return BadRequest("Item could not be added");
        }

        [HttpPost("get-item")]
        public IActionResult GetItem([FromQuery]string name)
        {
            if (string.IsNullOrEmpty(name)) { return BadRequest("item name can not be null"); }

            var Item = _foodItemServier.GetItem(name);

            if (Item == null) { return NotFound(); }
            return Ok(Item);
        }

        [HttpGet("get-all-item")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _foodItemServier.GetAll();
            if (items == null || items.Count == 0)
            {
                return NotFound("No items found.");
            }
            return Ok(items);
        }
    }
}
