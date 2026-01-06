using MinimalAPI.Models;
using System.Text.Json;

namespace MinimalAPI.RouteGroups;

public static class ProductsMapGroup
{
    private static List<Product> products = new () {
    new() {Id = 1, Name = "Smart Phone" },
    new() { Id = 2, Name = "Smart TV"}
};

    public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder group)
    {

        group.MapGet("/", async (HttpContext context) =>
        {
            await context.Response.WriteAsync(JsonSerializer.Serialize(products));
        });

        group.MapGet("/{id:int}", async (HttpContext context, int id) =>
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                context.Response.StatusCode = 404;

                await context.Response.WriteAsync("Product not found");

                return;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(product));
        });

        group.MapPost("/", async (HttpContext context, Product product) =>
        {
            products.Add(product);

            await context.Response.WriteAsync("Product added");
        });

        group.MapPut("/{id:int}", async (HttpContext context, int id, Product updatedProduct) =>
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            
            if (product is null)
            {
                context.Response.StatusCode = 404;

                await context.Response.WriteAsync("Product not found");
                
                return;
            }
            
            product.Name = updatedProduct.Name;
            
            await context.Response.WriteAsync("Product updated");
        });

        group.MapDelete("/{id:int}", async (HttpContext context, int id) =>
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            
            if (product is null)
            {
                context.Response.StatusCode = 404;
            
                await context.Response.WriteAsync("Product not found");
                
                return;
            }
            
            products.Remove(product);
            
            await context.Response.WriteAsync("Product deleted");
        });

        return group;
    }
}
