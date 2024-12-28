using Inno_Shop.BusinessLogic.Dto.Email;

namespace Inno_Shop.BusinessLogic.Services.EmailService;

public interface IEmailService
{ 
    Task SendEmail(Message message);
}