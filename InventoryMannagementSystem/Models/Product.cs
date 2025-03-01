using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMannagementSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product Name must be between 2 and 100 characters.")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Product Brand must be between 2 and 50 characters.")]
        public string ProductBrand { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity cannot be negative.")]
        public int StockQuantity { get; set; }

        [Required]
        [Range(100, 100000000, ErrorMessage = "Product Price must be in the range of 100 to 100000000")]
        public decimal ProductPrice { get; set; }

        //public int CategoryId{ get; set; }

        public Category Categories { get; set; }

    }
}
