using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.BusinessLogic.Dto.Product;

public class CreateProductDto
{
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    public string Title { get; set; }
    [Required]
    [Range(0, float.MaxValue, ErrorMessage = "Price must be a positive number.")]
    public float Cost { get; set; }
    public string? Description { get; set; }
}