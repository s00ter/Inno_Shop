using Inno_Shop.BusinessLogic.Dto.Product;
using Inno_Shop.DataAccess.Entities;

namespace Inno_Shop.BusinessLogic.Mappers;

public static class ProductMapper
{
    public static ProductInfoDto MapProductInfoDto(this Product product)
    {
        return new ProductInfoDto
        {
            Title = product.Title,
            Cost = product.Cost,
            Description = product.Description,
            UserName = product.User.UserName,
            CreatedAt = product.CreatedAt,
        };
    }
    
    public static ShowProductDto MapShowProductDto(this Product product)
    {
        return new ShowProductDto
        {
            Id = product.Id,
            UserId = product.UserId,
            Title = product.Title,
            Cost = product.Cost,
            Description = product.Description,
        };
    }
}