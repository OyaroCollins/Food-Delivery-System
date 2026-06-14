using CelticsRestaurantAPI.Data;
using CelticsRestaurantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CelticsRestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (await _context.Customers.AnyAsync(u => u.Email == model.Email) ||
                await _context.Admins.AnyAsync(a => a.Email == model.Email))
            {
                return BadRequest(new { message = "Email already exists." });
            }

            string passwordHash = HashPassword(model.Password);//A02: Cryptographic failures

            if (!string.IsNullOrEmpty(model.AdminCode))
            {
                string validAdminCode = _configuration["AdminSecretCode"];
                if (model.AdminCode != validAdminCode)
                {
                    return BadRequest(new { message = "Wrong admin code!" });
                }

                var admin = new Admin
                {
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = passwordHash
                };
                _context.Admins.Add(admin);
            }
            else
            {
                var customer = new Customer
                {
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = passwordHash
                };
                _context.Customers.Add(customer);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Registration successful!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {   //A03:Injection
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);

            if (admin != null && VerifyPassword(model.Password, admin.PasswordHash))
            {
                return Ok(new { id = admin.Id, name = admin.Name, email = admin.Email, role = "Admin", message = "Login successful!" });
            }
            else if (customer != null && VerifyPassword(model.Password, customer.PasswordHash))
            {
                return Ok(new { id = customer.Id, name = customer.Name, email = customer.Email, role = "Customer", message = "Login successful!" });
            }

            return Unauthorized(new { message = "Invalid credentials." });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
