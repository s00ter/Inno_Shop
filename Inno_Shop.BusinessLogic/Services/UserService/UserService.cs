using Inno_Shop.DataAccess;
using Microsoft.EntityFrameworkCore;
using Inno_Shop.BusinessLogic.Services.CurrentUserInfo;

namespace Inno_Shop.BusinessLogic.Services.UserService;

public class UserService(
    InnoShopContext context,
    ICurrentUserInfo currentUserInfo 
    ) : IUserService
{
    public async Task BlockUserAsync(string userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
        {
            throw new Exception("User not found");
        }
        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.DeletedBy = currentUserInfo.GetUserId();
        await context.SaveChangesAsync();
    }
}