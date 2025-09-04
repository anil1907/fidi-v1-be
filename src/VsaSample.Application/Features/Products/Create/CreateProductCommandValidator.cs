namespace VsaSample.Application.Features.Products.Create;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Sku"));

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage(CommonErrors.NonNegative(0));

        RuleFor(x => x.DefaultCulture)
            .NotEmpty().WithMessage(CommonErrors.PropNameIsRequired("DefaultCulture"));

        RuleFor(x => x.Translations)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Translations"))
            .Must((cmd, translations) => translations.Any(t => t.Culture == cmd.DefaultCulture))
            .WithMessage(CommonErrors.PropNameIsRequired("Translations"));

        RuleForEach(x => x.Translations).ChildRules(tr =>
        {
            tr.RuleFor(t => t.Culture)
                .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Culture"));

            tr.RuleFor(t => t.Name)
                .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Name"));
        });
    }
}

