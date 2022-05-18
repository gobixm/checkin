using Checkin.Service.Models.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Checkin.Service.Controllers;

[Route("api/about")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public sealed class AboutController : Controller
{
    private readonly UserManager<UserAccount> _userManager;

    public AboutController(UserManager<UserAccount> userManager)
    {
        _userManager = userManager;
    }


    [HttpGet("message")]
    public async Task<IActionResult> GetMessage()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return BadRequest();

        return Content($"{user.UserName} has been successfully authenticated.");
    }
}