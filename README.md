How to Run : 

1 : Clone the project => "git clone https://github.com/yourusername/ShopProject.git" 

2 : Change directory to the project => "cd ShopProject"

3 : Create a MySQL database and run the SQL schema in /Database/schema.sql

4 : Add a valid appsettings.json:
{
  "Jwt": {
    "Key": "your_secure_key",
    "Issuer": "your_app",
    "Audience": "your_app_users"
  },
  "ConnectionStrings": {
    "mysqlconnection": "Server=localhost;Database=shopdb;Uid=root;Pwd=yourpassword;"
  }
}

5 : Uncomment CORS piece of code in program.cs if it's needed (e.g., developing front-end for this project )

6 : Run the project => dotnet run
 
---------------------------------------------------------------------------------------------------------------------------------------

Available APIs:

| #  |       Endpoint        | Method |  Authorization Required  |                   Description                 |
| -- | --------------------- | ------ |  ---------------------   | ------------------------------------------    |
| 1  | `/login`              | POST   | ❌                      | Log in and receive JWT token                  |
| 2  | `/signup`             | POST   | ❌                      | Register a new user (Customer or Seller)      |
| 3  | `/order`              | POST   | ✅                      | Order a product (specify quantity)            |
| 4  | `/product/upload`     | POST   | ✅ (Seller)             | Upload a new product (Seller only)            |
| 5  | `/home`               | GET    | ❌                      | Get all available products (homepage)         |
| 6  | `/product/{id}`       | GET    | ❌                      | Get a product by its ID                       |
| 7  | `/product?search=...` | GET    | ❌                      | Search for products by name                   |
| 8  | `/product/rate`       | POST   | ✅                      | Rate and/or comment on a product              |
| 9  | `/product/update`     | POST   | ✅ (Owner Seller)       | Update a product (field + value) by the owner |
| 10 | `/wallet/deposit`     | POST   | ✅                      | Add money to your wallet                      |
| 11 | `/wallet/amount`      | GET    | ✅                      | Check current wallet balance                  |


---------------------------------------------------------------------------------------------------------------------------------------

 Testing :
 
 Use Postman and set:
 Headers => Authorization: Bearer <your_token>
 Body => Check raw , Select JSON 

 Note : For creating request body you can check the Models folder for realizing what data does each API need.

---------------------------------------------------------------------------------------------------------------------------------------
 Features :
 Signup/Login (with AES-encrypted passwords + JWT authentication)

 Product management (upload, update, search)

 Rating system (stars + optional comment)

 Wallet handling (add money, use in orders)

 Ordering system

 JWT-based Authorization

 Middlewares for input validation and logic handling

 Dapper-based Queries (no EF Core used)
 
 ---------------------------------------------------------------------------------------------------------------------------------------

Technologies Used :

|   Technology   |            Purpose             |
| -------------- | ------------------------------ |
| ASP.NET Core 9 | Web API backend framework      |
| Dapper         | Lightweight ORM for SQL        |
| MySQL          | Relational database            |
| AES Encryption | Password protection            |
| JWT            | Authentication + authorization |
| Postman        | API testing                    |

 ---------------------------------------------------------------------------------------------------------------------------------------
