using CelticsRestaurantAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NWebsec.AspNetCore.Middleware; // ✅ Add this line


var builder = WebApplication.CreateBuilder(args);

// ✅ Load Configuration
var configuration = builder.Configuration;

// ✅ Add Database Context (AppDbContext with Admin and Customer tables)
//A04: Insecure design
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 40))));  // Adjust MySQL version as needed

// ✅ Add Identity (Using default IdentityUser with custom roles)
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ✅ Enable Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Enable CORS (Allowing all origins in development, restrict in production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ✅ Add Controllers
builder.Services.AddControllers();

// ✅ Add Swagger (API Documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Build Application
var app = builder.Build();

// ✅ Enable Swagger in Development Mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // A05: Secure Configuration
    app.UseHsts(); 
    app.UseHttpsRedirection(); 
    app.UseXContentTypeOptions();
    app.UseXfo(options => options.Deny()); 
    app.UseXXssProtection(options => options.EnabledWithBlockMode()); 
}

// ✅ Enable Static Files (For serving images from wwwroot)
app.UseStaticFiles();

// ✅ Enable CORS
app.UseCors("AllowAll");

// ✅ Enable Session Middleware
app.UseSession();

// ✅ Enable Authentication Middleware (Session-based)
app.UseAuthentication(); // Ensure this is added before UseAuthorization

// ✅ Enable Routing & Authorization Middleware
app.UseRouting();
app.UseAuthorization();

// ✅ Map Controllers
app.MapControllers();

// ✅ Run Application
app.Run();
