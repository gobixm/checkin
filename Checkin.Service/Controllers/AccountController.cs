using Checkin.Service.Models.Authorizations;
using Checkin.Service.ViewModels.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Checkin.Service.Controllers;

[Authorize]
[Route("api/account")]
public class AccountController : Controller
{
    private readonly UserManager<UserAccount> _userManager;
    private readonly AccountDbContext _accountDbContext;

    public AccountController(UserManager<UserAccount> userManger, AccountDbContext accountDbContext)
    {
        _userManager = userManger;
        _accountDbContext = accountDbContext;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] UserAccountRegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByNameAsync(dto.Email);
        if (user != null)
        {
            return StatusCode(StatusCodes.Status409Conflict);
        }

        user = new UserAccount { UserName = dto.Email, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (result.Succeeded)
        {
            return Ok();
        }

        //todo: generic error handling
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return BadRequest(ModelState);
    }
}