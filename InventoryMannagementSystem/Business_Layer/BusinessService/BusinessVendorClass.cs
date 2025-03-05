using InventoryMannagementSystem.Business_Layer.BusinessInterface;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryMannagementSystem.Business_Layer.BusinessService
{
    public class BusinessVendorClass : IBusinessVendor
    {
        private readonly IVendor _vendor;
        private readonly IProduct _product;
        private readonly ICategory _category;

        public BusinessVendorClass(IVendor vendor, IProduct product, ICategory category)
        {
            _vendor = vendor;
            _product = product;
            _category = category;
        }

        public async Task<List<Vendor>> GetAllVendorAsync()
        {

            //return await _vendor.GetAllVendorAsync();

            return await _vendor.GetUpdatedStockAsync();
        }

        public async Task<string> DeleteVendorAsync(int id)
        {
            return await _vendor.DeleteVendorAsync(id);
        }

        public async Task<Vendor> GetVendorByIdAsync(int id)
        {
            return await _vendor.GetVendorByIdAsync(id);
        }

        public async Task<CVViewModel> InsertVendorAsync()
        {
            var categories = await _category.GetAllCategoryAsync();
            var products = await _product.GetAllProductsAsync();

            return new CVViewModel()
            {
                ModelType= "VendorModel",
                VendorModel = new Vendor(),
                CCategoryModel = categories,
                CProductModel = null,
            };
        }
            
        public async Task<string> InsertVendorAsync(CVViewModel vendor)
        {
            var newVendor = new Vendor()
            {
                VendorName = vendor.VendorModel.VendorName,
                VendorEmail = vendor.VendorModel.VendorEmail,
                VendorAddress = vendor.VendorModel.VendorAddress,
                Quantity = vendor.VendorModel.Quantity,
                Billing = vendor.VendorModel.Billing,
                DateOfPurchase = vendor.VendorModel.DateOfPurchase,
                Categories = new Category()
                {
                    CategoryId = vendor.ViewCategoryId,
                },
                Products = new Product()
                {
                    ProductId = vendor.ViewProductId,
                }
            };
            
            return await _vendor.InsertVendorAsync(newVendor);
        }

        public async Task<CVViewModel> UpdateVendorAsync(int id)
        {
            var vendor = await _vendor.GetVendorByIdAsync(id);
            var categories = await _category.GetAllCategoryAsync();
            var products = await _product.GetAllProductsAsync();

            return new CVViewModel()
            {
                ModelType = "VendorModel",
                CCategoryModel = categories,
                CProductModel = null,
                VendorModel = vendor,
                ViewCategoryId = vendor.Categories.CategoryId,
                ViewProductId = vendor.Products.ProductId
            };
        }

        public async Task<string> UpdateVendorAsync(CVViewModel vendor)
        {
            var updateVendor = new Vendor()
            {
                VendorId = vendor.VendorModel.VendorId,
                VendorName = vendor.VendorModel.VendorName,
                VendorEmail = vendor.VendorModel.VendorEmail,
                VendorAddress = vendor.VendorModel.VendorAddress,
                Quantity = vendor.VendorModel.Quantity,
                Billing = vendor.VendorModel.Billing,
                DateOfPurchase = vendor.VendorModel.DateOfPurchase,
                Categories = new Category()
                {
                    CategoryId = vendor.ViewCategoryId,
                },
                Products = new Product()
                {
                    ProductId = vendor.ViewProductId,
                }
            };

            return await _vendor.UpdateVendorByIdAsync(updateVendor);
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int v_CategoryId)
        {
            return await _product.GetProductsByCategoryAsync(v_CategoryId);
        }
    }
}
