using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CelticsRestaurantAPI.Models
{
    [Table("FoodItems")]
    public class FoodItem
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        // Dynamically generate the full image URL
        [NotMapped]
        public string FullImageUrl => $"http://localhost:5282/images/{ImageUrl}";

        // Relationship with OrderItems
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
