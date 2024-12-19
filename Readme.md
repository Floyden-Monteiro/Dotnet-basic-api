# BagAPI

## Project Description
BagAPI is a RESTful API designed to manage and organize bags and their contents. It allows users to create, read, update, and delete bags and items within those bags.

## Installation
To install and run the BagAPI locally, follow these steps:

1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/BagAPI.git
    ```
2. Navigate to the project directory:
    ```bash
    cd BagAPI
    ```

## Configuration
### Configure the Database
Update the `appsettings.json` file with your database connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=BagDB;User Id=your_user;Password=your_password;"
  },
  "JwtSettings": {
    "SecretKey": "your_secret_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience"
  }
}
```

### Apply Migrations
Run the following commands to apply the database migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Running the Application
Run the application using the following command:
```bash
dotnet run
```
The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## API Endpoints
### User Endpoints
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create a new user
- `PUT /api/users/{id}` - Update an existing user
- `DELETE /api/users/{id}` - Delete a user
- `POST /api/users/login` - Authenticate a user and get a JWT token

### Role Endpoints
- `GET /api/roles` - Get all roles
- `GET /api/roles/{id}` - Get role by ID
- `POST /api/roles` - Create a new role
- `PUT /api/roles/{id}` - Update an existing role
- `DELETE /api/roles/{id}` - Delete a role

### Product Endpoints
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create a new product
- `PUT /api/products/{id}` - Update an existing product
- `DELETE /api/products/{id}` - Delete a product

## Authentication
The API uses JWT tokens for authentication. To access protected endpoints, include the token in the Authorization header as follows:
```http
Authorization: Bearer <your_token>
```

## Migration Commands
- Creating a migration file: `dotnet ef migrations add InitialCreate`
- Attributing the migration to the database: `dotnet ef add AddAttributesToModels`
- Updating the database: `dotnet ef database update`
- Removing the last migration: `dotnet ef migrations remove`
- Update to a specific migration: `dotnet ef database update <Id of migration>`
- List of migrations: `dotnet ef migrations list`
- Add table to database: `dotnet ef migrations add AddProductTable`

## Postman Documentation
For detailed API documentation and testing, you can use the Postman collection available [here](https://documenter.getpostman.com/view/40139824/2sAYJ1mhbX).



