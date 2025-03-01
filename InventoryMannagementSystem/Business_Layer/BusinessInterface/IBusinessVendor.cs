using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryMannagementSystem.Business_Layer.BusinessInterface
{
    public interface IBusinessVendor
    {
        Task<List<Vendor>> GetAllVendorAsync();
        Task<CVViewModel> InsertVendorAsync();
        Task<string> InsertVendorAsync(CVViewModel vendor);
        Task<Vendor> GetVendorByIdAsync(int id);
        Task<string> DeleteVendorAsync(int id);
        Task<CVViewModel> UpdateVendorAsync(int id);
        Task<string> UpdateVendorAsync(CVViewModel vendor);
        Task<List<Product>> GetProductsByCategoryAsync(int v_CategoryId);
    }
}
