// Import required namespaces
using BagAPI.Helper;
using BagAPI.Services;
using BagAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BagAPI.RolesEndpoints
{
    public static class RolesEndpoints
    {
        public static void MapRolesEndpoint(this IEndpointRouteBuilder endpoints)
        {
            // GET All Roles
            endpoints.MapGet("/roles", async (RoleService roleService) =>
            {
                var response = await roleService.GetAllAsync();
                return response.Success ? Results.Ok(response) : Results.BadRequest(response);
            }).RequireAuthorization();

            // GET Role by ID
            endpoints.MapGet("/role", async (RoleService roleService, [FromHeader] string id) =>
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Results.BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
                }

                var response = await roleService.GetByIdAsync(id);
                return response.Success ? Results.Ok(response) : Results.NotFound(response);
            }).RequireAuthorization("AdminPolicy");

            // GET Users by Role
            endpoints.MapGet("/roles/{id}/users", async (RoleService roleService, string id, UserService userService) =>
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Results.BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
                }

                // Fetch the role by ID
                var roleResponse = await roleService.GetByIdAsync(id);
                if (!roleResponse.Success)
                {
                    return Results.NotFound(new ApiResponse<string>(false, "Role not found.", null));
                }

                // Fetch users for the given role
                var usersResponse = await userService.GetUsersByRoleIdAsync(id);
                return usersResponse.Success ? Results.Ok(usersResponse) : Results.NotFound(usersResponse);
            }).RequireAuthorization("AdminPolicy");

            // POST Create a New Role
            endpoints.MapPost("/roles", async (RoleService roleService, Roles role) =>
            {
                if (role == null)
                {
                    return Results.BadRequest(new ApiResponse<string>(false, "Invalid role data.", null));
                }

                var response = await roleService.CreateAsync(role);
                if (response.Success)
                {
                    return Results.Created($"/roles/{response.Data}", response);
                }

                return Results.BadRequest(response);
            }).RequireAuthorization("AdminPolicy");

            // PUT Update an Existing Role
            endpoints.MapPut("/roles/{id}", async (RoleService roleService, string id, Roles updatedRole) =>
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Results.BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
                }

                var response = await roleService.UpdateAsync(id, updatedRole);
                return response.Success ? Results.Ok(response) : Results.NotFound(response);
            }).RequireAuthorization("AdminPolicy");

            // DELETE a Role
            endpoints.MapDelete("/roles", async (RoleService roleService, [FromBody] DeleteRoleRequest request) =>
            {
                if (string.IsNullOrEmpty(request?.id))
                {
                    return Results.BadRequest(new ApiResponse<string>(false, "The 'id' body parameter is required.", null));
                }

                var response = await roleService.DeleteAsync(request.id);
                return response.Success ? Results.Ok(response) : Results.NotFound(response);
            }).RequireAuthorization("AdminPolicy");
        }
    }
}
