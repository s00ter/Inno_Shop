namespace Inno_Shop.BusinessLogic.Services.ProductContract;

public interface IProductApiContract
{
    Task DeleteByUserAsync(string userId);
}