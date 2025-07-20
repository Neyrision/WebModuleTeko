using FluentValidation;

namespace WebModuleTeko.Models.Validators;

public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{

    public RegisterUserModelValidator()
    {
        RuleFor(user => user);
    }

}
