using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMannagementSystem.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategory _category;

        public CategoryController(ICategory category)
        {
            _category = category;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _category.GetAllCategoryAsync());
        }

        [Route("InsertCategory")]
        [HttpGet]
        public IActionResult InsertCategory()
        {
            return View();
        }

        [HttpPost("InsertCategory")]
        public async Task<IActionResult> InsertCategory([FromForm] Category cat)
        {
            if (!ModelState.IsValid)
            {
                return View(cat);
            }

            ViewBag.Message = await _category.InsertCategoryAsync(cat);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var cat = await _category.GetCategoryByIdAsync(id);

            return View(cat);
        }

        [Route("DeleteCategory")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Message = await _category.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }

        [Route("UpdateCategory")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var cat = await _category.GetCategoryByIdAsync(id);
            return View(cat);
        }

        [Route("UpdateCategory")]
        [HttpPost]
        public async Task<IActionResult> Update(Category cat)
        {
            ViewBag.Message = await _category.UpdateCategoryAsync(cat);
            return RedirectToAction("Index");
        }

    }
}
