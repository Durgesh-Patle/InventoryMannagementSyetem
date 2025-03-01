using InventoryMannagementSystem.Models;

namespace InventoryMannagementSystem.Interface
{
    public interface IProduct
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<string> InsertProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task<string> DeleteProductAsync(int id);
        Task<string> UpdateProductAsync(Product product);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);

        //Task<List<Product>> GetProductByVendorAsync(int c_CategoryId);
    }
}
