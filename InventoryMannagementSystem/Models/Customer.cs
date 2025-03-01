using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryMannagementSystem.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Customer Name is required.")]
        [StringLength(100, ErrorMessage = "Customer Name cannot exceed 100 characters.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Invalid email format.")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string CustomerAddress { get; set; }

        public Category Categories { get; set; }
        public Product Products { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Total Billing Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Billing Amount must be greater than zero.")]
        public decimal TotalBillingAmount { get; set; }

        [Required(ErrorMessage = "Date of Purchase is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfPurchase { get; set; }
    }
}
