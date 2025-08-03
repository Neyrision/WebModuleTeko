using FluentValidation;

namespace WebModuleTeko.Models.Authentication.Validators;

public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{

    public RegisterUserModelValidator()
    {
        RuleFor(user => user);
    }

}
