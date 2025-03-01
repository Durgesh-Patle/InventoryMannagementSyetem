namespace InventoryMannagementSystem.Models
{
    public class ProductViewModel
    {
        public IEnumerable<Category> CategoryModel { get; set; }
        public Product ProductModel { get; set; }
        public int CategoryId { get; set; }

    }
}
