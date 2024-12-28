using Inno_Shop.DataAccess;
using Inno_Shop.DataAccess.Entities;
using Inno_Shop.DataAccess.Helpers;
using Inno_Shop.DataAccess.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

[TestClass]
public class ProductRepositoryTests
{
    private DbContextOptions<InnoShopContext> GetDbOptions(string dbName)
    {
        return new DbContextOptionsBuilder<InnoShopContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    [TestMethod]
    public async Task Add_ShouldAddProduct()
    {
        var options = GetDbOptions(Guid.NewGuid().ToString());
        using var context = new InnoShopContext(options);
        var repository = new ProductRepository(context);

        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Test Product",
            Cost = 100,
            CreatedBy = "TestUser",
            LastUpdatedBy = "TestUser",
            UserId = "User1"
        };

        await repository.Add(product);
        await context.SaveChangesAsync();

        var addedProduct = await context.Products.FindAsync(product.Id);
        Assert.IsNotNull(addedProduct);
    }

    [TestMethod]
    public async Task Update_ShouldUpdateProduct()
    {
        var options = GetDbOptions(Guid.NewGuid().ToString());
        using var context = new InnoShopContext(options);
        var repository = new ProductRepository(context);

        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Test Product",
            Cost = 100,
            CreatedBy = "TestUser",
            LastUpdatedBy = "TestUser",
            UserId = "User1"
        };

        await repository.Add(product);
        await context.SaveChangesAsync();

        product.Title = "Updated Product";
        await repository.Update(product);
        await context.SaveChangesAsync();

        var updatedProduct = await context.Products.FindAsync(product.Id);
        Assert.AreEqual("Updated Product", updatedProduct?.Title);
    }

    [TestMethod]
    public async Task Delete_ShouldRemoveProduct()
    {
        var options = GetDbOptions(Guid.NewGuid().ToString());
        using var context = new InnoShopContext(options);
        var repository = new ProductRepository(context);

        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Test Product",
            Cost = 100,
            CreatedBy = "TestUser",
            LastUpdatedBy = "TestUser",
            UserId = "User1"
        };

        await repository.Add(product);
        await context.SaveChangesAsync();

        await repository.Delete(product.Id);
        await context.SaveChangesAsync();

        var deletedProduct = await context.Products.FindAsync(product.Id);
        Assert.IsNull(deletedProduct);
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnFilteredProducts()
    {
        var options = GetDbOptions(Guid.NewGuid().ToString());
        using var context = new InnoShopContext(options);
        var repository = new ProductRepository(context);

        var product1 = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Test Product 1",
            Cost = 50,
            CreatedBy = "TestUser",
            LastUpdatedBy = "TestUser",
            UserId = "User1"
        };
        var product2 = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Test Product 2",
            Cost = 150,
            CreatedBy = "TestUser",
            LastUpdatedBy = "TestUser",
            UserId = "User1"
        };

        await repository.Add(product1);
        await repository.Add(product2);
        await context.SaveChangesAsync();

        var query = new QueryObject { From = 50, To = 150 };
        var result = await repository.GetAllAsync(query);

        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnProduct()
    {
        var options = GetDbOptions(Guid.NewGuid().ToString());
        using var context = new InnoShopContext(options);
        var repository = new ProductRepository(context);

        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Test Product",
            Cost = 100,
            CreatedBy = "TestUser",
            LastUpdatedBy = "TestUser",
            UserId = "User1"
        };

        await repository.Add(product);
        await context.SaveChangesAsync();

        Assert.IsNotNull(product);
        Assert.AreEqual("Test Product", product?.Title);
    }
}
