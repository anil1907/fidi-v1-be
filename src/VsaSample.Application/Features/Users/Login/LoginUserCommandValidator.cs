namespace VsaSample.Application.Features.Users.Login;

public class LoginUserCommandValidator: AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Username"))
            .NotNull().WithMessage(CommonErrors.PropNameNotNull("Username"))
            .MaximumLength(100).WithMessage(CommonErrors.PropNameMaxLength("Username", 100));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Password"))
            .NotNull().WithMessage(CommonErrors.PropNameNotNull("Password"))
            .MaximumLength(100).WithMessage(CommonErrors.PropNameMaxLength("Password", 100));
    }
}
