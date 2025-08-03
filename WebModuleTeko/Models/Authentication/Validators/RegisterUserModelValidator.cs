using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebModuleTeko.Database;

namespace WebModuleTeko.Models.Authentication.Validators;

public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{
    private readonly WmtContext _context;

    public RegisterUserModelValidator(WmtContext context)
    {
        _context = context;

        RuleFor(user => user)
            .MustAsync(MustNotHaveBeenTakenAsync);
    }

    private async Task<bool> MustNotHaveBeenTakenAsync(RegisterUserModel registerUserModel, CancellationToken cancellationToken)
    {
        var results = await _context.Users
            .Where(user => user.Email == registerUserModel.Email || user.Username == registerUserModel.Username)
            .ToListAsync();

        return results.Count <= 0;
    }

}
