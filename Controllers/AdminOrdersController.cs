using CelticsRestaurantAPI.Data;
using CelticsRestaurantAPI.DTOs;
using CelticsRestaurantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CelticsRestaurantAPI.Controllers
{
    [Route("api/admin/orders")]
    [ApiController]
    public class AdminOrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminOrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrders()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem) // Ensure FoodItem is properly included
                    .Include(o => o.Customer) // Ensure customer data loads
                    .Select(o => new OrderModel
                    {
                        Id = o.Id,
                        CustomerId = o.CustomerId,
                        CustomerName = o.Customer != null ? o.Customer.Name : "Unknown",
                        PhoneNumber = o.PhoneNumber,
                        Address = o.Address,
                        PaymentMethod = o.PaymentMethod,
                        DatePlaced = o.DatePlaced,
                        Status = o.Status,
                        TotalPrice = o.TotalPrice,
                        OrderItems = o.OrderItems.Select(oi => new OrderItemModel
                        {
                            FoodItemId = oi.FoodItemId,
                            FoodItemName = oi.FoodItem != null ? oi.FoodItem.Name : "Unknown",
                            Quantity = oi.Quantity,
                            Price = oi.Price // Ensure Price exists in OrderItems, not Orders
                        }).ToList()
                    }).ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching orders.", error = ex.Message });
            }
        }



        // PUT: api/admin/orders/{orderId}/status
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateStatusDto data)
        {
            if (string.IsNullOrEmpty(data.NewStatus))
            {
                return BadRequest(new { message = "Invalid status provided" });
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            order.Status = data.NewStatus;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Order ID {orderId} status updated to {data.NewStatus}." });
        }

        // DELETE: api/admin/orders/{orderId}
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Order ID {orderId} has been canceled." });
        }
    }
}
