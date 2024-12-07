namespace NutritionAppBackend.Models
{
    public class Nutrient
    {
        public int Id { get; set; }

        // Foreign key for FoodItem
        public int FoodItemId { get; set; }

        public double Fats { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }

        // Navigation property
        public FoodItem FoodItem { get; set; }
    }
}
