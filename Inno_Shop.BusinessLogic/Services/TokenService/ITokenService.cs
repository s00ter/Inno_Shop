using Inno_Shop.DataAccess.Entities;

namespace Inno_Shop.BusinessLogic.Services.TokenService;

public interface ITokenService
{
    string CreateToken(User user);
}