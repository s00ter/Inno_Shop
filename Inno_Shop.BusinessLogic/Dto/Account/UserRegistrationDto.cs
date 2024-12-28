using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.BusinessLogic.Dto.Account;

public class UserRegistrationDto
{
    [Required]
    [MinLength(5, ErrorMessage = "User Name must be at least 5 characters long.")]
    [MaxLength(100, ErrorMessage = "User Name cannot be longer than 100 characters.")]
    public string? Username { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    public string? ClientUri { get; set; }
}