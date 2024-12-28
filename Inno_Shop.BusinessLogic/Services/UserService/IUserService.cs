namespace Inno_Shop.BusinessLogic.Services.UserService;

public interface IUserService
{
    Task BlockUserAsync(string userId);
}