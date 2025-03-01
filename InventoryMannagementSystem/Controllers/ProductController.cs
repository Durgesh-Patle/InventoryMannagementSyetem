using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMannagementSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct _products;
        private readonly ICategory _category;

        public ProductController(IProduct product, ICategory category)
        {
            _products = product;
            _category = category;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _products.GetAllProductsAsync());
        }

        [Route("InsertProduct")]
        [HttpGet]
        public async Task<IActionResult> InsertProduct()
        {
            var categories = await _category.GetAllCategoryAsync();
            var viewModel = new ProductViewModel()
            {
                CategoryModel = categories,
                ProductModel=new Product()
            };
            return View(viewModel);

        }


        [Route("InsertProduct")]
        [HttpPost]
       // public async Task<IActionResult> InsertProduct([FromForm] Product product)
        public async Task<IActionResult> InsertProduct([FromForm] ProductViewModel product)
        {
            //var selectcatogory=await _category.GetCategoryByIdAsync(product.CategoryId);

            ModelState.Remove("CategoryModel");
            ModelState.Remove("ProductModel.Categories");

            if (!ModelState.IsValid)
            {
                product.CategoryModel = await _category.GetAllCategoryAsync();

                return View(product);
            }

            var newProduct = new Product()
            {
                ProductName = product.ProductModel.ProductName,
                ProductBrand = product.ProductModel.ProductBrand,
                StockQuantity = product.ProductModel.StockQuantity,
                ProductPrice = product.ProductModel.ProductPrice,
                Categories=new Category()
                {
                    CategoryId = product.CategoryId,
                }
            };

            ViewBag.Message = await _products.InsertProductAsync(newProduct);
            return RedirectToAction("Index");
        }


        [Route("GetProductById")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _products.GetProductByIdAsync(id);

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Message = await _products.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }

        [Route("UpdateProduct")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            //1.. Product Model Through
            //var product = await _products.GetProductByIdAsync(id);

            //ViewBag.Categories = await _category.GetAllCategoryAsync();
            //return View(product);


            //2.. View Model throw.
            var product = await _products.GetProductByIdAsync(id);

            var categories = await _category.GetAllCategoryAsync();

            var viewModel = new ProductViewModel()
            {
                ProductModel = product,
                CategoryModel = categories,
                CategoryId = product.Categories.CategoryId
            };
            return View(viewModel);
        }


        [Route("UpdateProduct")]
        [HttpPost]
        //public async Task<IActionResult> Update([FromForm] Product product)
        public async Task<IActionResult> Update(ProductViewModel product)
        {
            // 1...............
            //ViewBag.Message = await _products.UpdateProductAsync(product);
            //return RedirectToAction("Index");

            // 2.................
            var updatedProduct = new Product()
            {
                ProductId = product.ProductModel.ProductId,
                ProductName = product.ProductModel.ProductName,
                ProductBrand = product.ProductModel.ProductBrand,
                StockQuantity = product.ProductModel.StockQuantity,
                ProductPrice = product.ProductModel.ProductPrice,
                Categories = new Category()
                {
                    CategoryId = product.CategoryId
                }
            };

            ViewBag.Message = await _products.UpdateProductAsync(updatedProduct);
            return RedirectToAction("Index");
        }
    }
}
