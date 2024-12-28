namespace Inno_Shop.BusinessLogic.Services.ProductContract;

public class ProductApiContract(ProductApiOptions options) : IProductApiContract
{
    public async Task DeleteByUserAsync(string userId)
    {
        using var client = new HttpClient
        {
            BaseAddress = new Uri(options.Url)
        };
        
        await client.PatchAsync($"api/product/user/{userId}/delete", new StringContent(string.Empty));
    }
}