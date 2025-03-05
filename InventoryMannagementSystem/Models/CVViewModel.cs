namespace InventoryMannagementSystem.Models
{
    public class CVViewModel
    {
        public IEnumerable<Category> CCategoryModel { get; set; }
        public IEnumerable<Product> CProductModel { get; set; }
        public IEnumerable<Vendor> VendorList { get; set; }
        public Customer CustomerModel { get; set; }
        public Vendor VendorModel { get; set; }
        public string ModelType { get; set; }
        public int ViewCategoryId { get; set; }
        public int ViewProductId { get; set; }
        public int VendorProductId { get; set; }

        //public int SelectedVendorId { get; set; }
    }
}
