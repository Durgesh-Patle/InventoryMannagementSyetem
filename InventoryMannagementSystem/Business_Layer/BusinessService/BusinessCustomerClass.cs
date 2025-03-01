using InventoryMannagementSystem.Business_Layer.BusinessInterface;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryMannagementSystem.Business_Layer.BusinessService
{
    public class BusinessCustomerClass : IBusinessCustomer
    {
        private readonly ICustomer _customer;
        private readonly ICategory _category;
        private readonly IProduct _product;

        public BusinessCustomerClass(ICustomer customer, ICategory category, IProduct product)
        {
            _customer = customer;
            _category = category;
            _product = product;
        }

        public async Task<List<Customer>> GetAllCustomerAsync()
        {
            return await _customer.GetAllCustomerAsync();
        }

        public async Task<string> DeleteCustomerAsync(int id)
        {
            return await _customer.DeleteCustomerAsync(id);
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _customer.GetCustomerByIdAsync(id);
        }

        public async Task<CVViewModel> InsertCustomerAsync()
        {
            var categories = await _category.GetAllCategoryAsync();

            return new CVViewModel()
            {
                ModelType = "CustomerModel",
                CustomerModel = new Customer(),
                CCategoryModel = categories,
                CProductModel = null,
            };
        }

        public async Task<string> InsertCustomerAsync(CVViewModel customer)
        {
            var newCustomer = new Customer()
            {
                CustomerName = customer.CustomerModel.CustomerName,
                CustomerEmail = customer.CustomerModel.CustomerEmail,
                CustomerAddress = customer.CustomerModel.CustomerAddress,
                Quantity = customer.CustomerModel.Quantity,
                TotalBillingAmount = customer.CustomerModel.TotalBillingAmount,
                DateOfPurchase = customer.CustomerModel.DateOfPurchase,
                Categories = new Category()
                {
                    CategoryId = customer.ViewCategoryId,
                },
                Products = new Product()
                {
                    ProductId = customer.ViewProductId,
                }
            };

            var products = await _product.GetAllProductsAsync();
            if (products == null)
            {
                return "Products Not Found!!";
            }

            var existingProduct = products.FirstOrDefault(p => p.ProductId == newCustomer.Products.ProductId);

            if (existingProduct != null)
            {
                int upadateQuintity = (int)existingProduct.StockQuantity;

                if (existingProduct.StockQuantity > newCustomer.Quantity)
                {
                     upadateQuintity = (int)(existingProduct.StockQuantity - newCustomer.Quantity);
                }

                var newProduct = new Product
                {
                    ProductId = existingProduct.ProductId,
                    ProductName = existingProduct.ProductName,
                    ProductPrice = existingProduct.ProductPrice,
                    ProductBrand = existingProduct.ProductBrand,
                    StockQuantity = upadateQuintity,
                    Categories = new Category()
                    {
                        CategoryId = newCustomer.Categories.CategoryId
                    }
                };
                var res = await _product.UpdateProductAsync(newProduct);
            }

            return await _customer.InsertCustomerAsync(newCustomer);
        }

        public async Task<CVViewModel> UpdateCustomerAsync(int id)
        {
            var customer = await _customer.GetCustomerByIdAsync(id);
            var categories = await _category.GetAllCategoryAsync();

            return new CVViewModel()
            {
                ModelType = "CustomerModel",
                CCategoryModel = categories,
                CProductModel = null,
                CustomerModel = customer,
                ViewCategoryId = customer.Categories.CategoryId,
                ViewProductId = customer.Products.ProductId
            };
        }

        public async Task<string> UpdateCustomerAsync(CVViewModel customer)
        {
            var updateCustomer = new Customer()
            {
                CustomerId = customer.CustomerModel.CustomerId,
                CustomerName = customer.CustomerModel.CustomerName,
                CustomerEmail = customer.CustomerModel.CustomerEmail,
                CustomerAddress = customer.CustomerModel.CustomerAddress,
                Quantity = customer.CustomerModel.Quantity,
                TotalBillingAmount = customer.CustomerModel.TotalBillingAmount,
                DateOfPurchase = customer.CustomerModel.DateOfPurchase,
                Categories = new Category()
                {
                    CategoryId = customer.ViewCategoryId,
                },
                Products = new Product()
                {
                    ProductId = customer.ViewProductId,
                }
            };

            return await _customer.UpdateCustomerByIdAsync(updateCustomer);
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int v_CategoryId)
        {
            return await _product.GetProductsByCategoryAsync(v_CategoryId);
        }
    }
}
