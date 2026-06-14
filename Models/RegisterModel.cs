using System.ComponentModel.DataAnnotations;

namespace CelticsRestaurantAPI.Models
{
    public class RegisterModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string? AdminCode { get; set; } // Admins must enter a secret code
    }
}
