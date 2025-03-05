namespace InventoryMannagementSystem.Models
{
    public class VendorProducts
    {
        public int VendorProductId { get; set; }
        public Vendor Vendor { get; set; }
        public Category Category { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Billing { get; set; }
        public DateTime DateOfSale { get; set; }
    }
}
