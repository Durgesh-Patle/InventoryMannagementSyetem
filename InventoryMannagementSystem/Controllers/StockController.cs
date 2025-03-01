using InventoryMannagementSystem.Business_Layer.BusinessInterface;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryMannagementSystem.Controllers
{
    public class StockController : Controller
    {
        private readonly IVendor _vendor;
        private readonly IProduct _product;
        private readonly ICategory _category;

        public StockController(IVendor vendor, IProduct product, ICategory category)
        {
            _vendor = vendor;
            _product = product;
            _category = category;
        }

        [HttpGet]
        public async Task<IActionResult> StockUpdate()
        {
            var vendors = await _vendor.GetAllVendorAsync();

            var viewModel = new CVViewModel()
            {
                VendorList = vendors,
                CCategoryModel = new List<Category>(),
                CProductModel = new List<Product>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StockUpdate([FromForm] CVViewModel stock)
        {

            var products = await _product.GetAllProductsAsync();

            var existingProduct = products.FirstOrDefault(p => p.ProductId == stock.ViewProductId);

            if (existingProduct != null)
            {
                var existingVendorStock = await _vendor.GetAllVendorAsync();
                var vendorStock = existingVendorStock.FirstOrDefault(v => v.VendorId == stock.VendorModel.VendorId);

                if (vendorStock != null)
                {
                    existingProduct.Categories.CategoryId = stock.ViewCategoryId;
                    existingProduct.StockQuantity += stock.VendorModel.Quantity;
                    await _product.UpdateProductAsync(existingProduct);
                }
            }


            //Vendor Agar Exist krta hai To Purane recourd me hi add Ho Jaye If Not Exist To New Vendor Create.
            var allVendors = await _vendor.GetAllVendorAsync();

            var vend = await _vendor.GetVendorByIdAsync(stock.VendorModel.VendorId);

            var existingVendor = allVendors.FirstOrDefault(v =>
                v.VendorId == stock.VendorModel.VendorId && v.Products.ProductId == stock.ViewProductId);

            if (existingVendor != null)
            {
                existingVendor.Billing = stock.VendorModel.Billing;
                existingVendor.Quantity = stock.VendorModel.Quantity;
                existingVendor.Categories = new Category { CategoryId = stock.ViewCategoryId };
                existingVendor.Products = new Product { ProductId = stock.ViewProductId };

                await _vendor.UpdateVendorByIdAsync(existingVendor);
            }
            else
            {
                var newVendor = new Vendor
                {
                    VendorName = vend.VendorName,
                    VendorAddress = vend.VendorAddress,
                    VendorEmail = vend.VendorEmail,
                    Billing = stock.VendorModel.Billing,
                    DateOfPurchase = vend.DateOfPurchase,  
                    Quantity = stock.VendorModel.Quantity,
                    Categories = new Category { CategoryId = stock.ViewCategoryId },
                    Products = new Product { ProductId = stock.ViewProductId }
                };

                await _vendor.InsertVendorAsync(newVendor);  
            }

            return RedirectToAction("Index", "Product");
        }


        [HttpGet]
        public async Task<JsonResult> GetCategoriesByVendor(int vendorId)
        {
            var categories = await _category.GetCategoriesByVendorAsync(vendorId);
            return Json(categories);
        }

        [HttpGet]
        public async Task<JsonResult> GetProductsByCategory(int categoryId)
        {
            var products = await _product.GetProductsByCategoryAsync(categoryId);
            return Json(products);
        }

    }
}
