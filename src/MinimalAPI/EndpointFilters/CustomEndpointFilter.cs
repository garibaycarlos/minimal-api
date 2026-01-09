using MinimalAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.EndpointFilters;

public class CustomEndpointFilter(ILogger<CustomEndpointFilter> logger) : IEndpointFilter
{
    private readonly ILogger<CustomEndpointFilter> _logger = logger;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        _logger.LogInformation("Endpoint filter - before logic");

        var product = context.Arguments.OfType<Product>().FirstOrDefault();

        if (product is null) return Results.BadRequest("Product details were not found in the request");

        var validationContext = new ValidationContext(product);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

        if (!isValid) return Results.BadRequest(validationResults.FirstOrDefault()?.ErrorMessage);

        var result = await next(context); // invoke the subsecuent filter (if any) or endpoint's request delegate

        _logger.LogInformation("Endpoint filter - after logic");

        return result;
    }
}
