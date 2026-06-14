using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CelticsRestaurantAPI.Models
{
    [Table("OrderItems")]
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int FoodItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        // Relationships
        [ForeignKey("OrderId")]
        public required Order Order { get; set; }

        [ForeignKey("FoodItemId")]
        public required FoodItem FoodItem { get; set; }
    }
}
