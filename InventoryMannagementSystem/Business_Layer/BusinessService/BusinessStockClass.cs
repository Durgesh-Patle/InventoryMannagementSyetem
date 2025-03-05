using System.Reflection;
using InventoryMannagementSystem.Business_Layer.BusinessInterface;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;

namespace InventoryMannagementSystem.Business_Layer.BusinessService
{
    public class BusinessStockClass : IBusinessStock
    {
        private readonly IVendor _vendor;
        private readonly IProduct _product;
        private readonly ICategory _category;

        public BusinessStockClass(IVendor vendor, IProduct product, ICategory category)
        {
            _vendor = vendor;
            _product = product;
            _category = category;
        }

        public async Task<CVViewModel> GetStockUpdateViewModelAsync()
        {
            var vendors = await _vendor.GetAllVendorAsync();
            return new CVViewModel()
            {
                VendorList = vendors,
                CCategoryModel = new List<Category>(),
                CProductModel = new List<Product>()
            };
        }

        //public async Task UpdateStockAsync(CVViewModel stock)
        //{
          
        //}

        public async Task<IEnumerable<Category>> GetCategoriesByVendorAsync(int vendorId)
        {
            return await _category.GetCategoriesByVendorAsync(vendorId);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _product.GetProductsByCategoryAsync(categoryId);
        }
    }
}
