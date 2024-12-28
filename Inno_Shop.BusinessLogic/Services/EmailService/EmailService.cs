using Inno_Shop.BusinessLogic.Dto.Email;
using MailKit.Net.Smtp;
using MimeKit;

namespace Inno_Shop.BusinessLogic.Services.EmailService;

public class EmailService(EmailConfiguration emailConfiguration) : IEmailService
{
    public async Task SendEmail(Message message)
    {
        var emailMassage = CreateEmailMessage(message);
        
        await SendAsync(emailMassage);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        
        emailMessage.From.Add(new MailboxAddress(string.Empty,emailConfiguration.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.Content,
        };

        if (message.Attachments != null && message.Attachments.Any())
        {
            foreach (var attachment in message.Attachments)
            {
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    attachment.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
            }
        }
        
        emailMessage.Body = bodyBuilder.ToMessageBody();
        
        return emailMessage;
    }

    private async Task SendAsync(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(emailConfiguration.UserName, emailConfiguration.Password);

            await client.SendAsync(mailMessage);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}