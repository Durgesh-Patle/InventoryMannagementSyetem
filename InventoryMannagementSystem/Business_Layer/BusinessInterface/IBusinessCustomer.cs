using InventoryMannagementSystem.Models;

namespace InventoryMannagementSystem.Business_Layer.BusinessInterface
{
    public interface IBusinessCustomer
    {
        Task<List<Customer>> GetAllCustomerAsync();
        Task<CVViewModel> InsertCustomerAsync();
        Task<string> InsertCustomerAsync(CVViewModel customer);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<string> DeleteCustomerAsync(int id);
        Task<CVViewModel> UpdateCustomerAsync(int id);
        Task<string> UpdateCustomerAsync(CVViewModel customer);
        Task<List<Product>> GetProductsByCategoryAsync(int v_CategoryId);
    }
}
