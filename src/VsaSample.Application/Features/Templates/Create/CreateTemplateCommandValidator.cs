namespace VsaSample.Application.Features.Templates.Create;

internal sealed class CreateTemplateCommandValidator : AbstractValidator<CreateTemplateCommand>
{
    public CreateTemplateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Name"))
            .MaximumLength(256).WithMessage(CommonErrors.PropNameMaxLength("Name", 256));

        RuleFor(x => x.Description)
            .MaximumLength(1024).When(x => x.Description is not null)
            .WithMessage(CommonErrors.PropNameMaxLength("Description", 1024));

        RuleForEach(x => x.Sections)
            .ChildRules(section =>
            {
                section.RuleFor(s => s.Title)
                    .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Section Title"))
                    .MaximumLength(128).WithMessage(CommonErrors.PropNameMaxLength("Section Title", 128));

                section.RuleForEach(s => s.Items)
                    .ChildRules(item =>
                    {
                        item.RuleFor(i => i.Label)
                            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Item Label"))
                            .MaximumLength(128).WithMessage(CommonErrors.PropNameMaxLength("Item Label", 128));
                        item.RuleFor(i => i.Amount)
                            .MaximumLength(64).WithMessage(CommonErrors.PropNameMaxLength("Amount", 64));
                        item.RuleFor(i => i.Note)
                            .MaximumLength(256).When(i => i.Note is not null)
                            .WithMessage(CommonErrors.PropNameMaxLength("Note", 256));
                        item.RuleFor(i => i.Calories)
                            .GreaterThanOrEqualTo(0).When(i => i.Calories.HasValue)
                            .WithMessage(CommonErrors.NonNegative(0));
                    });
            });
    }
}
