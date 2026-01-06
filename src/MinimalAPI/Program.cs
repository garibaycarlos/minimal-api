using MinimalAPI.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var products = new List<Product> {
    new() {Id = 1, Name = "Smart Phone" },
    new() { Id = 2, Name = "Smart TV"}
};

app.MapGet("/products", async (HttpContext context) =>
{
    await context.Response.WriteAsync(JsonSerializer.Serialize(products));
});

app.MapGet("/products/{id:int}", async (HttpContext context, int id) =>
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

app.MapPost("/products", async (HttpContext context, Product product) =>
{
    products.Add(product);

    await context.Response.WriteAsync("Product added");
});

app.Run();
