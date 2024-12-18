using BagAPI.Helper;
using BagAPI.Models;

public static class UsersEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // GET All Users (Accessible by "user" and "admin")
        endpoints.MapGet("/users", async (UserService userService) =>
        {
            var response = await userService.GetAllAsync();
            return response.Success ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization("UserPolicy");

        // GET User by ID (Accessible by "user" and "admin")
        endpoints.MapGet("/users/{id}", async (UserService userService, string id) =>
        {
            if (string.IsNullOrEmpty(id))
            {
                return Results.BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
            }

            var response = await userService.GetByIdAsync(id);
            return response.Success ? Results.Ok(response) : Results.NotFound(response);
        }).RequireAuthorization("UserPolicy");

        // POST Create a New User (Accessible by "admin" only)
        endpoints.MapPost("/users", async (UserService userService, Users user) =>
        {
            if (user == null)
            {
                return Results.BadRequest(new ApiResponse<string>(false, "Invalid user data.", null));
            }

            user.RoleId = "1"; // Assign the default role to the user

            var response = await userService.CreateAsync(user);
            if (response.Success)
            {
                return Results.Created($"/users/{response.Data}", response);
            }

            return Results.BadRequest(response);
        }).RequireAuthorization("AdminPolicy");

        // PUT Update an Existing User (Accessible by "admin" only)
        endpoints.MapPut("/users/{id}", async (UserService userService, string id, Users updatedUser) =>
        {
            if (string.IsNullOrEmpty(id))
            {
                return Results.BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
            }

            var response = await userService.UpdateAsync(id, updatedUser);
            return response.Success ? Results.Ok(response) : Results.NotFound(response);
        }).RequireAuthorization("AdminPolicy");

        // DELETE a User (Accessible by "admin" only)
        endpoints.MapDelete("/users/{id}", async (UserService userService, string id) =>
        {
            if (string.IsNullOrEmpty(id))
            {
                return Results.BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
            }

            var response = await userService.DeleteAsync(id);
            return response.Success ? Results.Ok(response) : Results.NotFound(response);
        }).RequireAuthorization("AdminPolicy");

        // Login Route (Accessible by everyone)
        endpoints.MapPost("/login", async (UserService userService, LoginRequest loginRequest) =>
        {
            if (string.IsNullOrEmpty(loginRequest.email) || string.IsNullOrEmpty(loginRequest.password))
            {
                return Results.BadRequest(new ApiResponse<string>(false, "Email and password are required.", null));
            }

            var token = userService.Authenticate(loginRequest.email, loginRequest.password);
            if (token == null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new { Token = token });
        });
    }
}
