using CelticsRestaurantAPI.Data;
using CelticsRestaurantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CelticsRestaurantAPI.DTOs;

namespace CelticsRestaurantAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderModel orderModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            if (orderModel.OrderItems == null || orderModel.OrderItems.Count == 0)
            {
                return BadRequest(new { message = "Cart cannot be empty." });
            }

            // Retrieve food items from the database
            var foodItemIds = orderModel.OrderItems.Select(oi => oi.FoodItemId).ToList();
            var foodItems = await _context.FoodItems
                .Where(f => foodItemIds.Contains(f.Id))
                .ToListAsync();

            if (foodItems.Count != foodItemIds.Count)
            {
                return BadRequest(new { message = "One or more food items do not exist." });
            }

            // Calculate total price
            decimal totalPrice = orderModel.OrderItems.Sum(item =>
            {
                var foodItem = foodItems.FirstOrDefault(f => f.Id == item.FoodItemId);
                return foodItem != null ? foodItem.Price * item.Quantity : 0;
            });

            // Create the order
            var order = new Order
            {
                CustomerId = orderModel.CustomerId,
                PhoneNumber = orderModel.PhoneNumber,
                Address = orderModel.Address,
                PaymentMethod = orderModel.PaymentMethod,
                TotalPrice = totalPrice,
            };

            // Assign Order and FoodItem to each OrderItem
            order.OrderItems = orderModel.OrderItems.Select(item =>
            {
                var foodItem = foodItems.FirstOrDefault(f => f.Id == item.FoodItemId);
                return new OrderItem
                {
                    FoodItemId = item.FoodItemId,
                    Quantity = item.Quantity,
                    Order = order, // ✅ Assign Order reference
                    FoodItem = foodItem // ✅ Assign FoodItem reference
                };
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Order placed successfully!",
                orderId = order.Id,
                totalPrice = totalPrice
            });
        }

    }
}
