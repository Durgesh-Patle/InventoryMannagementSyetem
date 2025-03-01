using InventoryMannagementSystem.Models;

namespace InventoryMannagementSystem.Interface
{
    public interface ICustomer
    {
        Task<List<Customer>> GetAllCustomerAsync();
        Task<string> InsertCustomerAsync(Customer vendor);
        Task<string> DeleteCustomerAsync(int id);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<string> UpdateCustomerByIdAsync(Customer vendor);
    }
}
