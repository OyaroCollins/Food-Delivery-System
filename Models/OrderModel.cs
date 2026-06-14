using System;
using System.Collections.Generic;

namespace CelticsRestaurantAPI.DTOs
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = "Unknown"; // Instead of full Customer object
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "Cash";
        public DateTime DatePlaced { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Processing";
        public decimal TotalPrice { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }

    public class OrderItemModel
    {
        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
