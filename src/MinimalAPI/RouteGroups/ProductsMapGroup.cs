using MinimalAPI.Models;
using System.Text.Json;

namespace MinimalAPI.RouteGroups;

public static class ProductsMapGroup
{
    private static List<Product> products = [
        new() {Id = 1, Name = "Smart Phone" },
        new() { Id = 2, Name = "Smart TV"}
    ];

    public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder group)
    {
        // GET /products
        group.MapGet("/", () => products); // short version omitting return Results.Ok(products)

        // GET /products/{id}
        group.MapGet("/{id:int}", (int id) =>
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return Results.NotFound("Product not found");
            }

            return Results.Ok(product);
        });

        // POST /products
        group.MapPost("/", async (Product product) =>
        {
            products.Add(product);

            return Results.Ok("Product added");
        });

        // PUT /products/{id}
        group.MapPut("/{id:int}", async (int id, Product updatedProduct) =>
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return Results.NotFound("Product not found");
            }

            product.Name = updatedProduct.Name;

            return Results.Ok("Product updated");
        });

        // DELETE /products/{id}
        group.MapDelete("/{id:int}", async (int id) =>
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                // slight variation here to return a JSON response
                return Results.NotFound(new { error = "Product not found" });
            }

            products.Remove(product);

            // slight variation here to return a JSON response
            return Results.Ok(new { message = "Product deleted" });
        });

        return group;
    }
}
