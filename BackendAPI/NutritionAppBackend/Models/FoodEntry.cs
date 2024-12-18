﻿namespace NutritionAppBackend.Models
{
    public class FoodEntry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FoodName { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
        public DateTime DateAdded { get; set; }

        public User User { get; set; }  
    }


}
