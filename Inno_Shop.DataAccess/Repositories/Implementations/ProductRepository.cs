using System.Linq.Expressions;
using Inno_Shop.DataAccess.Entities;
using Inno_Shop.DataAccess.Helpers;
using Inno_Shop.DataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop.DataAccess.Repositories.Implementations;

public class ProductRepository(InnoShopContext context) : IProductRepository
{
    public async Task<Product> Add(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
        return product;
    }

    public Task UpdateRange(List<Product> products)
    {
        context.Products.UpdateRange(products);
        return context.SaveChangesAsync();
    }

    public async Task<Product?> Delete(string id)
    {
        var res = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (res == null)
        {
            return null;
        }
        context.Products.Remove(res);
        await context.SaveChangesAsync();
        return res;
    }

    public async Task<List<Product>> GetAllAsync(QueryObject query)
    {
        var products = context.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            products = products.Where(x => x.Title.Contains(query.Title));
        }

        if (query.From.HasValue)
        {
            products = products.Where(x => x.Cost >= query.From);
        }
        
        if (query.To.HasValue)
        {
            products = products.Where(x => x.Cost <= query.To);
        }

        if (query.PriceAscending.HasValue)
        {
            if (query.PriceAscending.Value)
            {
                products = products.OrderBy(x => x.Cost);
            }
            else
            {
                products = products.OrderByDescending(x => x.Cost);
            }
        }
        
        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        
        return await products.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>> predicate)
    {
        var query = context.Products.Where(predicate);
        
        return await query.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        var res =  await context.Products
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
        return res;
    }
}