namespace Inno_Shop.BusinessLogic.Dto.Product;

public class ProductInfoDto
{
    public string Title { get; set; }
    public float Cost { get; set; }
    public string? Description { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
}