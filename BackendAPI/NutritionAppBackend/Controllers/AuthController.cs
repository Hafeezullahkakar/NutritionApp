using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutritionAppBackend.Data;
using NutritionAppBackend.DTOs;
using NutritionAppBackend.Models;
using NutritionAppBackend.Services;
using System.Security.Cryptography;
using System.Text;

namespace NutritionAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly NutritionDbContext _dbContext;
        private readonly JwtHelper _jwtTokenHelper;

        public AuthController(NutritionDbContext dbContext)
        {
            _dbContext = dbContext;
            _jwtTokenHelper = new JwtHelper("This is hafeez secret for a reason that he will not change come what may and he will always be there for all of us");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == registrationDto.Email))
                return BadRequest("Email is already registered.");
            if (!registrationDto.Goal.Equals("loss") && !registrationDto.Goal.Equals("gain"))
                return BadRequest("Please enter the correct goal. It must be either 'loss' or 'gain'.");

            var newUser = new User
            {
                Name = registrationDto.Name,
                Email = registrationDto.Email,
                Password = HashPassword(registrationDto.Password),
                Weight = registrationDto.Weight,
                Height = registrationDto.Height,
                Age = registrationDto.Age,
                Goal = registrationDto.Goal,
            };

            // Calculate Maintenance and Goal Calories
            newUser.MaintenanceCalories = CalculateMaintenanceCalories(newUser.Weight, newUser.Height, newUser.Age);
            newUser.GoalCalories = CalculateGoalCalories(newUser.MaintenanceCalories, newUser.Goal);

            // Calculate daily macronutrient needs
            var protein = UserNeededProtein(newUser.Weight);
            var fats = UserNeededFats(newUser.GoalCalories);
            var carbs = UserNeededCarbs(newUser.GoalCalories, protein, fats);

            newUser.DailyProtein = protein;
            newUser.DailyFats = fats;
            newUser.DailyCarbs = carbs;

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
                return Unauthorized("Invalid email or password.");

            var token = _jwtTokenHelper.GenerateToken(user.Id, user.Email);
            return Ok(new { Token = token});

        }

        [HttpGet("getuserdetails")]
        public async Task<IActionResult> GetUserDetails([FromQuery] int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound("User not found");
              
            return Ok(new { User = user });
        }

        [HttpPost("updateDailyIntake")]
        public async Task<IActionResult> UpdateDailyIntake([FromQuery] int userId, [FromBody] NutritionDto dailyNutrition)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found");

            // Deduct the calculated nutrition from the user's daily remaining values
            user.DailyCaloriesRemaining -= dailyNutrition.Calories;
            user.DailyProtein -= dailyNutrition.Protein;
            user.DailyCarbs -= dailyNutrition.Carbs;
            user.DailyFats -= dailyNutrition.Fats;

            // Ensure the remaining values don't go negative
            user.DailyCaloriesRemaining = Math.Max(0, user.DailyCaloriesRemaining);
            user.DailyProtein = Math.Max(0, user.DailyProtein);
            user.DailyCarbs = Math.Max(0, user.DailyCarbs);
            user.DailyFats = Math.Max(0, user.DailyFats);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "User's daily intake updated successfully." });
        }


        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private static bool VerifyPassword(string inputPassword, string storedPassword)
        {
            var hashedInput = HashPassword(inputPassword);
            return hashedInput == storedPassword;
        }

        private static double CalculateMaintenanceCalories(double weight, double height, int age)
        {
            return (10 * weight + 6.25 * height - 5 * age + 5) * 1.6; // Mifflin-St Jeor Equation for males
        }

        private static double CalculateGoalCalories(double maintenanceCalories, string goal)
        {
            return goal == "loss" ? maintenanceCalories - 500 : maintenanceCalories + 500; // Adjust calories for goal
        }
        private static double UserNeededProtein(double weight)
        {
            // Use 1.7g protein per kg of body weight as a default
            return Math.Round(1.7 * weight, 2);
        }

        private static double UserNeededFats(double goalCalories)
        {
            // Use 25% of total goal calories for fats, divided by 9 (calories per gram of fat)
            return Math.Round((0.225 * goalCalories) / 9, 2);
        }

        private static double UserNeededCarbs(double goalCalories, double protein, double fats)
        {
            // Remaining calories for carbs after protein and fats
            var remainingCalories = goalCalories - ((protein * 4) + (fats * 9));
            return Math.Round(remainingCalories / 4, 2); // Divide by 4 (calories per gram of carbs)
        }

    }
}
