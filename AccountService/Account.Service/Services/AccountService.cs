using Checkin.AccountService.Contracts;
using Checkin.AccountService.Domain.Models;
using Checkin.AccountService.Repositories.Repositories;
using Checkin.AccountService.Service.Models;
using Checkin.AccountService.Service.Validators;
using Checkin.Common.Extensions;
using Checkin.Common.Validation.Extensions;
using Gobi.Outboxes.Services.Services;
using Gobi.UnitOfWorks.Abstractions;
using Mapster;

namespace Checkin.AccountService.Service.Services;

public sealed class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IOutboxPublisher _outboxPublisher;
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(
        IUnitOfWork unitOfWork,
        IOutboxPublisher outboxPublisher)
    {
        _unitOfWork = unitOfWork;
        _outboxPublisher = outboxPublisher;
        _accountRepository = unitOfWork.GetRepository<IAccountRepository>();
    }

    public async Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest createRequest,
        CancellationToken cancellationToken = default)
    {
        var account = NormalizeCreateAccountRequest(createRequest).Adapt<Account>();
        account.Id = Guid.NewGuid();

        await new CreateAccountRequestValidator(_accountRepository).ValidateThrowAsync(createRequest,
            cancellationToken);

        account = await _accountRepository.AddAsync(account, cancellationToken);
        await _outboxPublisher.PublishAsync(new AccountCreatedEvent(account.Id,
            account.Interests?.ToArray()));

        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateAccountResponse(account.Login, account.Name, account.Id);
    }

    public async Task DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken = default)
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
        return request with
        {
            Login = NormalizeLogin(request.Login)
        };
    }

    private static string NormalizeLogin(string login)
    {
        return login.TrimLower();
    }
}