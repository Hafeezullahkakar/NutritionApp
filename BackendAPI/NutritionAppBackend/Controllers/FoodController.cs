using Microsoft.AspNetCore.Mvc;
using NutritionAppBackend.Data;
using NutritionAppBackend.Models;
using NutritionAppBackend.Services;

namespace NutritionAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly NutritionDbContext _dbContext;

        public FoodController(NutritionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       

        [HttpPost("confirm-food")]
        public async Task<IActionResult> ConfirmFood([FromBody] ConfirmFoodRequest request)
        {
            if (request == null || request.Calories <= 0)
                return BadRequest("Valid food information is required.");

            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound("User not found");

            // Update user's remaining intake
            user.DailyCaloriesRemaining -= request.Calories;
            user.DailyProtein += request.Protein;
            user.DailyCarbs += request.Carbs;
            user.DailyFats += request.Fats;

            // Log the food entry
            var foodEntry = new FoodEntry
            {
                UserId = user.Id,
                FoodName = request.Food,
                Calories = request.Calories,
                Protein = request.Protein,
                Carbs = request.Carbs,
                Fats = request.Fats,
                DateAdded = DateTime.UtcNow
            };

            _dbContext.FoodEntries.Add(foodEntry);
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                message = "Food entry added and intake updated.",
                remainingCalories = user.DailyCaloriesRemaining
            });
        }
    }
}

public class AddFoodRequest
{
    public int UserId { get; set; } // The ID of the user adding the food
    public string Food { get; set; } // The name of the food item
}

public class ConfirmationResponse
{
    public string Food { get; set; } // The name of the food item
    public double Calories { get; set; } // Calories of the food
    public double Protein { get; set; } // Protein content of the food
    public double Carbs { get; set; } // Carbohydrate content of the food
    public double Fats { get; set; } // Fat content of the food
}

public class ConfirmFoodRequest
{
    public int UserId { get; set; } // The ID of the user confirming the food
    public string Food { get; set; } // The name of the food item
    public double Calories { get; set; } // Calories of the food
    public double Protein { get; set; } // Protein content of the food
    public double Carbs { get; set; } // Carbohydrate content of the food
    public double Fats { get; set; } // Fat content of the food
}
