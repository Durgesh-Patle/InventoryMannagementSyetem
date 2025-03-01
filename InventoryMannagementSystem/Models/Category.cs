using System.ComponentModel.DataAnnotations;

namespace InventoryMannagementSystem.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 100 characters.")]
        public string CategoryName { get; set; }
    }
}
