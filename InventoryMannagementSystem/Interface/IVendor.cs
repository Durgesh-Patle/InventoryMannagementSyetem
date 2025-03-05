using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMannagementSystem.Interface
{
    public interface IVendor
    {
        Task<List<Vendor>> GetAllVendorAsync();
        Task<List<Vendor>> GetUpdatedStockAsync();
        Task<string> InsertVendorAsync(Vendor vendor);
        Task<string> DeleteVendorAsync(int id);
        Task<Vendor> GetVendorByIdAsync(int id);
        Task<string> UpdateVendorByIdAsync(Vendor vendor);
    }
}
