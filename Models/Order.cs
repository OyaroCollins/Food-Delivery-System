using System;
using System.Collections.Generic;

namespace CelticsRestaurantAPI.Models
{
    public class Order
    {
        public int Id { get; set; } // Primary key

        // Foreign Key for Customer
        public int CustomerId { get; set; }

        // Navigation Property

        public string CustomerName { get; set; } = string.Empty;
        public Customer? Customer { get; set; }

        // Basic details
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "Cash";
        public DateTime DatePlaced { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Processing";
        public decimal TotalPrice { get; set; }

        // Order Items
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
