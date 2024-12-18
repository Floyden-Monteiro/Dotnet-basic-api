using BagAPI.Helper;
using BagAPI.Services;
using BagAPI.Models;


namespace BagAPI.ProductsEndpoints{
    public static class ProductsEndpoints{
        public static void MapProductsEndpoints(this IEndpointRouteBuilder endpoints){
            // GET All Products
            endpoints.MapGet("/products", async (ProductService productService) =>
            {
                var response = await productService.GetAllAsync();
                return response.Success ? Results.Ok(response) : Results.BadRequest(response);
            });

            // GET Product by ID
            endpoints.MapGet("/products/{id}", async (ProductService productService, string id) =>
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Results.BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
                }

                var response = await productService.GetByIdAsync(id);
                return response.Success ? Results.Ok(response) : Results.NotFound(response);
            });

            // POST Create a New Product
            endpoints.MapPost("/products", async (ProductService productService, Products product) =>
            {
                if (product == null)
                {
                    return Results.BadRequest(new ApiResponse<string>(false, "Invalid product data.", null));
                }

                var response = await productService.CreateAsync(product);
                if (response.Success)
                {
                    return Results.Created($"/products/{response.Data.id}", response);
                }

                return Results.BadRequest(response);
            });

            // PUT Update an Existing Product
            endpoints.MapPut("/products/{id}", async (ProductService productService, string id, Products updatedProduct) =>
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Results.BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
                }

                var response = await productService.UpdateAsync(id, updatedProduct);
                return response.Success ? Results.Ok(response) : Results.NotFound(response);
            });

            // DELETE a Product
            endpoints.MapDelete("/products/{id}", async (ProductService productService, string id) =>
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Results.BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
                }

                var response = await productService.DeleteAsync(id);
                return response.Success ? Results.Ok(response) : Results.NotFound(response);
            });
        }
    }
}