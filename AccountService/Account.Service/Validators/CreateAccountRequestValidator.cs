using Checkin.AccountService.Repository.Repositories;
using Checkin.AccountService.Service.Models;
using FluentValidation;

namespace Checkin.AccountService.Service.Validators;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator(IAccountRepository accountRepository)
    {
        RuleFor(x => x.Login)
            .MustAsync(async (login, ct) => await accountRepository.GetLoginExistsAsync(login, ct) is false)
            .WithMessage("Login not unique");
    }
}