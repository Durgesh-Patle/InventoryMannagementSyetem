using InventoryMannagementSystem.Business_Layer.BusinessInterface;
using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryMannagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IBusinessCustomer _businessCustomer;
        public CustomerController(IBusinessCustomer businessCustomer)
        {
            _businessCustomer = businessCustomer;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _businessCustomer.GetAllCustomerAsync();
            return View(customers);
        }

        [Route("InsertCustomer")]
        [HttpGet]
        public async Task<IActionResult> InsertCustomer()
        {
            var viewModel = await _businessCustomer.InsertCustomerAsync();
            return View(viewModel);
        }

        [Route("InsertCustomer")]
        [HttpPost]
        public async Task<IActionResult> InsertCustomer(CVViewModel customer)
        {
            ModelState.Remove("CCategoryModel");
            ModelState.Remove("ModelType");
            ModelState.Remove("CProductModel");
            ModelState.Remove("VendorModel");
            ModelState.Remove("CustomerModel.Categories");
            ModelState.Remove("CustomerModel.Products");
            ModelState.Remove("SelectedVendorId");
            ModelState.Remove("VendorList");
            ModelState.Remove("Vendor.VendorProductId");

            if (!ModelState.IsValid)
            {
                var cust = await _businessCustomer.InsertCustomerAsync();
                return View(cust);
            }

            await _businessCustomer.InsertCustomerAsync(customer);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Message = await _businessCustomer.DeleteCustomerAsync(id);
            return RedirectToAction("Index");
        }

        [Route("GetCustomerById")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _businessCustomer.GetCustomerByIdAsync(id);
            return View(customer);
        }

        [Route("UpdateCustomer")]
        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var viewModel = await _businessCustomer.UpdateCustomerAsync(id);
            return View(viewModel);
        }

        [Route("UpdateCustomer")]
        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(CVViewModel customer)
        {
            ViewBag.Message = await _businessCustomer.UpdateCustomerAsync(customer);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<JsonResult> GetProductsByCategory(int v_CategoryId)
        {
            var products = await _businessCustomer.GetProductsByCategoryAsync(v_CategoryId);
            return Json(products);
        }
    }
}
