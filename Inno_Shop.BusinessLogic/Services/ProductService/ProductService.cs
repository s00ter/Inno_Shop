using Inno_Shop.BusinessLogic.Dto.Product;
using Inno_Shop.BusinessLogic.Mappers;
using Inno_Shop.BusinessLogic.Services.CurrentUserInfo;
using Inno_Shop.DataAccess.Entities;
using Inno_Shop.DataAccess.Helpers;
using Inno_Shop.DataAccess.Repositories.Abstractions;

namespace Inno_Shop.BusinessLogic.Services.ProductService;

public class ProductService(
    IProductRepository productRepository,
    ICurrentUserInfo currentUserInfo
    ) : IProductService
{
    public async Task<List<ShowProductDto>> GetAllProducts(QueryObject query)
    {
        var products = await productRepository.GetAllAsync(query);
        var res = products.Where(x=> x.IsAvailable).Select(x => x.MapShowProductDto()).ToList();
        return res;
    }
    
    public async Task<Product> CreateProduct(CreateProductDto dto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            UserId = currentUserInfo.GetUserId(),
            Title = dto.Title,
            Cost = dto.Cost,
            Description = dto.Description,
            IsDeleted = false,
            IsAvailable = true,
            DeletedAt = null,
            DeletedBy = null,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserInfo.GetUserId(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = currentUserInfo.GetUserId(),
        };
            
        return await productRepository.Add(product);
    }
    
    public async Task<ShowProductDto> UpdateProduct(string id, UpdateProductDto dto)
    {
        var dbProd = await productRepository.GetByIdAsync(id);
        if (dbProd == null)
        { 
            throw new NullReferenceException("Product not found");
        }
        if (dbProd.UserId != currentUserInfo.GetUserId())
        { 
            throw new Exception("Product not yours");
        }
        
        dbProd.Title = dto.Title;
        dbProd.Cost = dto.Cost;
        dbProd.Description = dto.Description;
        dbProd.LastUpdatedAt = DateTime.UtcNow;
        dbProd.LastUpdatedBy = currentUserInfo.GetUserId();
        
        var res = await productRepository.Update(dbProd);

        return res.MapShowProductDto();
    }
    
    public async Task<Product?> DeleteProduct(string id)
    {
        var user = currentUserInfo.GetUserId();
        var dbProduct = await productRepository.Delete(id);
        if (dbProduct == null)
        {
            throw new NullReferenceException("Product not found");
        }
        if (dbProduct.UserId != user)
        {
            throw new Exception("You do not have permission to delete this product");
        }
        return dbProduct;
    }

    public async Task<ProductInfoDto> GetProductInfo(string id)
    {
       var prod = await productRepository.GetByIdAsync(id);
       if (prod is null)
       {
           throw new NullReferenceException("Product not found");
       }
       return prod.MapProductInfoDto();
    }

    public async Task DeleteProductsByUserId(string userId)
    {
        var dbProducts = await productRepository.GetAllAsync(x => x.UserId == userId);

        foreach (var product in dbProducts)
        {
            product.IsDeleted = true;
        }
        
        await productRepository.UpdateRange(dbProducts);
    }
}