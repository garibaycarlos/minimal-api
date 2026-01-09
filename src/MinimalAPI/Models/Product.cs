using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models;

public class Product
{
    [Required (ErrorMessage = "Id cannot be blank")]
    [Range(0, int.MaxValue, ErrorMessage = "Id must be between 0 and the maximum value of int")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Name cannot be blank")]
    public string Name { get; set; } = string.Empty;

    public override string ToString() => $"Product Id: {Id}, Product Name: {Name}";
}
