using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMannagementSystem.Models
{
    public class Vendor
    {
        public int VendorId { get; set; }

        [Required]
        [StringLength(100,MinimumLength =3, ErrorMessage = "Vendor Name cannot exceed 100 characters")]
        public string VendorName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
         ErrorMessage = "Invalid email format.")]
        public string VendorEmail { get; set; }

        [Required]
        //[RegularExpression(@"^[a-zA-Z0-9\s,.-]{5,100}$",
            //ErrorMessage = "Address must be 5-100 characters long and contain only letters, numbers, spaces, commas, dots, and hyphens.")]
        public string VendorAddress { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Billing amount must be non-negative")]
        public decimal Billing { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime DateOfPurchase { get; set; }
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
        public Product Products { get; set; }
    }
}
