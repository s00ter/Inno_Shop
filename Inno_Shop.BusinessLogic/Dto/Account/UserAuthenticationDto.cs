using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.BusinessLogic.Dto.Account;

public class UserAuthenticationDto
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
    
}