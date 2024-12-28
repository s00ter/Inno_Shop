using System.Linq.Expressions;
using Inno_Shop.DataAccess.Entities;
using Inno_Shop.DataAccess.Helpers;

namespace Inno_Shop.DataAccess.Repositories.Abstractions;

public interface IProductRepository
{
    Task<Product> Add(Product product);
    Task<Product> Update(Product product);
    Task UpdateRange(List<Product> products);
    Task<Product?> Delete(string id);
    Task<List<Product>> GetAllAsync(QueryObject query);
    Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>> predicate);
    Task<Product?> GetByIdAsync(string id);
}