using InventoryMannagementSystem.Business_Layer.BusinessInterface;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryMannagementSystem.Controllers
{
    public class VendorController : Controller
    {
        private readonly IBusinessVendor _businessVendor;

        public VendorController(IBusinessVendor businessVendor)
        {
            _businessVendor = businessVendor;
        }

        public async Task<IActionResult> Index()
        {
            var vendors = await _businessVendor.GetAllVendorAsync();
            return View(vendors);
        }

        [Route("InsertVendor")]
        [HttpGet]
        public async Task<IActionResult> InsertVendor()
        {
            var viewModel = await _businessVendor.InsertVendorAsync();
            return View(viewModel);
        }

        [Route("InsertVendor")]
        [HttpPost]
        public async Task<IActionResult> InsertVendor(CVViewModel vendor)
        {
            ModelState.Remove("CCategoryModel");
            ModelState.Remove("ModelType");
            ModelState.Remove("CustomerModel");
            ModelState.Remove("CProductModel");
            ModelState.Remove("VendorModel.Categories");
            ModelState.Remove("VendorModel.Products");
            ModelState.Remove("SelectedVendorId");
            ModelState.Remove("VendorList");

            if (!ModelState.IsValid)
            {
                var ven = await _businessVendor.InsertVendorAsync();
                return View(ven);
            }

            await _businessVendor.InsertVendorAsync(vendor);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Message = await _businessVendor.DeleteVendorAsync(id);
            return RedirectToAction("Index");
        }

        [Route("GetVendorById")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var vendor = await _businessVendor.GetVendorByIdAsync(id);
            return View(vendor);
        }

        [Route("UpdateVendorById")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var viewModel = await _businessVendor.UpdateVendorAsync(id);
            return View(viewModel);
        }

        [Route("UpdateVendorById")]
        [HttpPost]
        public async Task<IActionResult> Update(CVViewModel vendor)
        {
            ViewBag.Message = await _businessVendor.UpdateVendorAsync(vendor);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<JsonResult> GetProductsByCategory(int v_CategoryId)
        {
            var products = await _businessVendor.GetProductsByCategoryAsync(v_CategoryId);
            return Json(products);
        }
    }
}
