namespace Inno_Shop.BusinessLogic.Dto.Product;

public class ShowProductDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public float Cost { get; set; }
    public string? Description { get; set; }
}