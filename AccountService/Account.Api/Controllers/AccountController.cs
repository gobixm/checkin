using Checkin.AccountService.Service.Models;
using Checkin.AccountService.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Checkin.AccountService.Api.Controllers;

[ApiController]
[Route("account")]
public sealed class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<CreateAccountResponse> CreateAccountAsync(
        [FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken)
    {
        return await _accountService.CreateAccountAsync(request, cancellationToken);
    }

    [HttpDelete("{id:guid}")]
    public async Task DeleteAccountAsync(Guid id, CancellationToken cancellationToken)
    {
        await _accountService.DeleteAccountAsync(id, cancellationToken);
    }

    [HttpGet("uniq_login/{login}")]
    public async Task<bool> IsLoginUnique(string login, CancellationToken cancellationToken)
    {
        return await _accountService.IsLoginExistsAsync(login, cancellationToken) is false;
    }
}