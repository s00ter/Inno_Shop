using Inno_Shop.BusinessLogic.Dto.Product;
using Inno_Shop.DataAccess.Entities;
using Inno_Shop.DataAccess.Helpers;

namespace Inno_Shop.BusinessLogic.Services.ProductService;

public interface IProductService
{
    Task<List<ShowProductDto>> GetAllProducts(QueryObject query);
    Task<Product> CreateProduct(CreateProductDto dto);
    Task<ShowProductDto> UpdateProduct(string id, UpdateProductDto dto);
    Task<Product?> DeleteProduct(string id);
    Task<ProductInfoDto> GetProductInfo(string id);
    Task DeleteProductsByUserId(string userId);
}