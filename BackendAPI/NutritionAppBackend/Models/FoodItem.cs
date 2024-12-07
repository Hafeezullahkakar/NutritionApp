namespace NutritionAppBackend.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double? Weight { get; set; }
        public int? Count { get; set; }
        public string Identifier { get; set; }

        // Navigation property for nutrients
        public Nutrient Nutrient { get; set; }
    }
}
