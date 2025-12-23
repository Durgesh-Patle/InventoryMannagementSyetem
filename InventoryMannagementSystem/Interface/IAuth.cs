using InventoryMannagementSystem.Models;

namespace InventoryMannagementSystem.Interface
{
    public interface IAuth
    {
        Task<ResponceModel> InsertRegisterUser(AuthModel user);
        Task<ResponceModel> ValidateUser(AuthModel user);
        Task<List<AuthModel>> GetUserDetails(AuthModel user);
    }
}
