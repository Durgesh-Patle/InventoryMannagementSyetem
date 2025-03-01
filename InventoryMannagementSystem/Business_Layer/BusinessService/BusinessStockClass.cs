using InventoryMannagementSystem.Business_Layer.BusinessInterface;
using InventoryMannagementSystem.Interface;

namespace InventoryMannagementSystem.Business_Layer.BusinessService
{
    public class BusinessStockClass
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



    }
}
