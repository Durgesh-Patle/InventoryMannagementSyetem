using InventoryMannagementSystem.Business_Layer.BusinessInterface;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryMannagementSystem.Controllers
{
    public class StockController : Controller
    {
        private readonly IBusinessStock _businessStock;
        private readonly IStock _stock;

        public StockController(IBusinessStock businessStock , IStock stock)
        {
            _businessStock = businessStock;
            _stock = stock;
        }

        [HttpGet]
        public async Task<IActionResult> StockUpdate()
        {
            var viewModel = await _businessStock.GetStockUpdateViewModelAsync();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StockUpdate([FromForm] CVViewModel stock)
        {
            await _stock.UpdateStockAsync(stock);
            return RedirectToAction("Index", "Vendor");
        }

        [HttpGet]
        public async Task<JsonResult> GetCategoriesByVendor(int vendorId)
        {
            var categories = await _businessStock.GetCategoriesByVendorAsync(vendorId);
            return Json(categories);
        }

        [HttpGet]
        public async Task<JsonResult> GetProductsByCategory(int categoryId)
        {
            var products = await _businessStock.GetProductsByCategoryAsync(categoryId);
            return Json(products);
        }
    }
}
