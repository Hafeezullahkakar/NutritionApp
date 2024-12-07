namespace NutritionAppBackend.DTOs
{
    public class UserRegistrationDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public string Goal { get; set; } // "loss" or "gain"
    }
 

}
