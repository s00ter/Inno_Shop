using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.BusinessLogic.Dto.Account;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? ClientUri { get; set; }
}