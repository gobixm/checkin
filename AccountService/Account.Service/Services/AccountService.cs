using Checkin.AccountService.Domain.Models;
using Checkin.AccountService.Repository.Repositories;
using Checkin.AccountService.Service.Models;
using Checkin.AccountService.Service.Validators;
using Checkin.Common.Extensions;
using Checkin.Common.Repositories.Repositories;
using Checkin.Common.Validation.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Mapster;

namespace Checkin.AccountService.Service.Services;

public sealed class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _accountRepository = unitOfWork.GetRepository<IAccountRepository>();
    }

    public async Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest createRequest,
        CancellationToken cancellationToken = default)
    {
        var account = NormalizeCreateAccountRequest(createRequest).Adapt<Account>();

        await new CreateAccountRequestValidator(_accountRepository).ValidateThrowAsync(createRequest, cancellationToken);
        
        account = await _accountRepository.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateAccountResponse(account.Login, account.Name, account.Id);
    }

    public async Task DeleteAccountAsync(int accountId, CancellationToken cancellationToken = default)
    {
        _accountRepository.Remove(accountId);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task<bool> IsLoginExistsAsync(string login, CancellationToken cancellationToken = default)
    {
        return await _accountRepository.GetLoginExistsAsync(NormalizeLogin(login), cancellationToken);
    }

    private static CreateAccountRequest NormalizeCreateAccountRequest(CreateAccountRequest request)
    {
        return request with { Login = NormalizeLogin(request.Login) };
    }

    private static string NormalizeLogin(string login)
    {
        return login.TrimLower();
    }
}