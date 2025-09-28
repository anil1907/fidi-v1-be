namespace VsaSample.Application.Features.Clients.Update;

internal sealed class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("FirstName"));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("LastName"));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Email"))
            .EmailAddress();

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Phone"));

        When(x => x.Goals is not null, () =>
        {
            RuleForEach(x => x.Goals!)
                .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Goal"))
                .MaximumLength(128).WithMessage(CommonErrors.PropNameMaxLength("Goal", 128));
        });
    }
}
