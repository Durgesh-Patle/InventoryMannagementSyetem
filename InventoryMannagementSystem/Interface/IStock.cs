using InventoryMannagementSystem.Models;

namespace InventoryMannagementSystem.Interface
{
    public interface IStock
    {
        Task<string> UpdateStockAsync(CVViewModel stock);
    }
}
