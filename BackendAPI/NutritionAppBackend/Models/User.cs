namespace NutritionAppBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public string Goal { get; set; }
        public double MaintenanceCalories { get; set; }
        public double GoalCalories { get; set; }
        public double DailyCaloriesRemaining { get; set; }
        public double DailyProtein { get; set; }
        public double DailyCarbs { get; set; }
        public double DailyFats { get; set; }
    }


}
