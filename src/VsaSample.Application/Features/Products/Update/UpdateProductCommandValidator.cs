namespace VsaSample.Application.Features.Products.Update;

internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Name"));

        RuleFor(x => x.Culture)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Culture"));
    }
}

