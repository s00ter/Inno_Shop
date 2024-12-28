using Inno_Shop.BusinessLogic.Constants;
using Inno_Shop.BusinessLogic.Dto.Account;
using Inno_Shop.BusinessLogic.Dto.Email;
using Inno_Shop.BusinessLogic.Services.EmailService;
using Inno_Shop.BusinessLogic.Services.ProductContract;
using Inno_Shop.BusinessLogic.Services.TokenService;
using Inno_Shop.BusinessLogic.Services.UserService;
using Inno_Shop.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(
    UserManager<User> userManager,
    ITokenService tokenService,
    IEmailService emailService,
    IUserService userService,
    IProductApiContract productApiContract
    ) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto)
    {
        var appUser = new User
        {
            UserName = userRegistrationDto.Username,
            Email = userRegistrationDto.Email,
        };
        
        var createResult = await userManager.CreateAsync(appUser, userRegistrationDto.Password!);
        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors.Select(e => e.Description);
            return BadRequest(new RegistrationResponseDto { Errors = errors});
        }
        
        var roleResult = await userManager.AddToRoleAsync(appUser, RoleConstants.User);
        if (!roleResult.Succeeded)
        {
            var errors = roleResult.Errors.Select(e => e.Description);
            return BadRequest(new RegistrationResponseDto { Errors = errors});
        }
        
        var token = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
        /*var param = new Dictionary<string, string?>
        {
            { "token", token },
            { "email", appUser.Email!}
        };
        
        var callback = QueryHelpers.AddQueryString(userRegistrationDto.ClientUri!, param);*/
        
        var message = new Message([appUser.Email], "Email confirmation token", token, null);
        
        await emailService.SendEmail(message);
        
        return Ok(new { Message = "Email confirmation token sent successfully" });
    }

    [HttpPost("email-verification")]
    public async Task<IActionResult> EmailVerification(string? email, string? token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        {
            return BadRequest("Invalid payload");
        }
        
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return BadRequest("Invalid email verification");
        }
        
        var isEmailVerified = await userManager.ConfirmEmailAsync(user, token);
        if (!isEmailVerified.Succeeded)
        {
            return BadRequest("Invalid email verification");
        }
        return Ok(new { Message = "Email confirmed" });
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDto userAuthenticationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = await userManager.FindByNameAsync(userAuthenticationDto.Username!);
        if (user == null)
        {
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid password" });
        }

        var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
        if (!isEmailConfirmed)
        {
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Email not confirmed" });
        }
        
        var isCorrect = await userManager.CheckPasswordAsync(user, userAuthenticationDto.Password!);
        if (!isCorrect)
        {
            await userManager.AccessFailedAsync(user);
            
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid password" });
        }
        
        var userStatus = await userManager.IsLockedOutAsync(user);
        if (userStatus)
        {
            return Unauthorized(new AuthResponseDto{ErrorMessage = "User is locked out please try again later."});
        }
        
        await userManager.ResetAccessFailedCountAsync(user);

        var token = tokenService.CreateToken(user);
        
        return Ok(new AuthResponseDto
        {
            IsAuthSuccessful = true,
            Token = token
        });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user  = await userManager.FindByEmailAsync(forgotPasswordDto.Email!);
        if (user is null)
        {
            return BadRequest(new { Message = "Invalid email address" });
        }
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        /*var param = new Dictionary<string, string?>
        {
            { "token", token },
            { "email", forgotPasswordDto.Email!}
        };
        
        var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientUri!, param);*/
        
        var message = new Message([user.Email], "Reset password token", token, null);
        
        await emailService.SendEmail(message);
        
        return Ok(new { Message = "Password reset sent successfully" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user  = await userManager.FindByEmailAsync(resetPasswordDto.Email!);
        if (user is null)
        {
            return BadRequest(new { Message = "Invalid email address" });
        }
        
        var res = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token!, resetPasswordDto.Password!);
        if (!res.Succeeded)
        {
            return BadRequest(res.Errors.Select(e => e.Description));
        }

        return Ok(new { Message = "Password reset sent successfully" });
    }

    [HttpPatch("block-user")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> BlockUser(string userId)
    {
        await userService.BlockUserAsync(userId);
        await productApiContract.DeleteByUserAsync(userId);
        
        return Ok(new { Message = "User is blocked" });
    }
}