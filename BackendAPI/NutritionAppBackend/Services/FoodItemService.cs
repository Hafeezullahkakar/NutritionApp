using Microsoft.EntityFrameworkCore;
using NutritionAppBackend.Data;
using NutritionAppBackend.Models;

namespace NutritionAppBackend.Services
{
    public class FoodItemService
    {
        private readonly NutritionDbContext _context;

        public FoodItemService(NutritionDbContext context)
        {
            _context = context;
        }

        public async Task<FoodItem> AddFoodItem(FoodItem item)
        {

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Food item cannot be null");
            }

           await _context.FoodItem.AddAsync(item);

            await _context.SaveChangesAsync();

            return item;

        }

        public async Task<FoodItem> GetItem(string name)
        {
            return await _context.FoodItem.Include(fi => fi.Nutrient).Where(n => n.Name == name).FirstOrDefaultAsync();         
        }

        public async Task<List<FoodItem>> GetAll()
        {

            return await _context.FoodItem.Include(s=>s.Nutrient).ToListAsync();

        }


    }

}
