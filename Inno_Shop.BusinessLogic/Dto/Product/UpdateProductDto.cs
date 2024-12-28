using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.BusinessLogic.Dto.Product;

public class UpdateProductDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public float Cost { get; set; }
    public string? Description { get; set; }
}