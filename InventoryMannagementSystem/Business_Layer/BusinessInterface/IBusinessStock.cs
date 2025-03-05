using InventoryMannagementSystem.Models;

namespace InventoryMannagementSystem.Business_Layer.BusinessInterface
{
    public interface IBusinessStock
    {
        Task<CVViewModel> GetStockUpdateViewModelAsync();

        //Task UpdateStockAsync(CVViewModel stock);
        Task<IEnumerable<Category>> GetCategoriesByVendorAsync(int vendorId);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    }
}
