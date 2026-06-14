Online Fast Food Delivery System (CELTICS Restaurant)
 Project Overview

The Online Fast Food Delivery System is a web-based application developed for CELTICS Restaurant in Karen, Nairobi (hypothetical).
It allows customers to browse food items, place orders, and track order status, while administrators manage food items and orders efficiently.

The system is built using ASP.NET Core Web API, C#, MySQL, and basic frontend technologies (HTML, CSS, JavaScript).

🛠️ Technologies Used
Backend: ASP.NET Core Web API (C#)
Database: MySQL
Frontend: HTML, CSS, JavaScript
ORM: Entity Framework Core
Tools: Visual Studio, VS Code, MySQL Workbench
⭐ Features
👤 Customer Features
User Registration and Login
Browse food menu
Add items to cart
Remove items from cart
Place orders
Choose payment method (Cash on Delivery / M-Pesa simulation)
View order status (Processing, Ready, Out for Delivery)
🛠️ Admin Features
Admin Login
Add new food items
Edit existing food items
Delete food items
Manage customer orders
Update order status
🧠 System Architecture
Frontend (HTML, CSS, JavaScript)
        ↓
ASP.NET Core Web API (C#)
        ↓
MySQL Database
🚀 How to Run the Project
1. Clone the repository
git clone https://github.com/your-username/fastfood-system.git
2. Setup Backend
Open project in Visual Studio
Restore NuGet packages
Configure database connection in appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=fastfooddb;user=root;password=yourpassword;"
}
Run database migrations:
dotnet ef database update
Start the API:
dotnet run
3. Setup Frontend
Open home.html in browser OR use Live Server in VS Code
Ensure API URL matches backend (e.g., https://localhost:5282/api)
📊 Database Structure (Main Tables)
Users
FoodItems
Orders
OrderItems
🏠 Home Page

(Add screenshot here)

🍽️ Menu Page

(Add screenshot here)

🛒 Cart Page

(Add screenshot here)

🧾 Checkout Page

(Add screenshot here)

🛠️ Admin Dashboard

(Add screenshot here)

🎯 Future Improvements
M-Pesa real payment integration
Real-time order tracking system
SMS/email notifications
Mobile app for delivery riders
Cloud deployment (Azure / AWS)
👨‍💻 Developer

Name: Collins Oyaro
Course: Computer Science
Project: School Project

📌 Note

This project was built for learning purposes and demonstrates full-stack development skills using ASP.NET Core and MySQL.
