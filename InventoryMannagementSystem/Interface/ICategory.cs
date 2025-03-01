using InventoryMannagementSystem.Models;

namespace InventoryMannagementSystem.Interface
{
    public interface ICategory
    {
        Task<List<Category>> GetAllCategoryAsync();
        Task<string> InsertCategoryAsync(Category cat);
        Task<string> UpdateCategoryAsync(Category cat);
        Task<string> DeleteCategoryAsync(int id);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<List<Category>> GetCategoriesByVendorAsync(int c_VendorId);
    }
}
