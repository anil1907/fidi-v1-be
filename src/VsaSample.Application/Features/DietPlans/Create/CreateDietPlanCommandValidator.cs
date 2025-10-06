using VsaSample.Application.Features.DietPlans.Shared;

namespace VsaSample.Application.Features.DietPlans.Create;

internal sealed class CreateDietPlanCommandValidator : AbstractValidator<CreateDietPlanCommand>
{
    public CreateDietPlanCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.TemplateId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Name"))
            .MaximumLength(256).WithMessage(CommonErrors.PropNameMaxLength("Name", 256));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage(CommonErrors.PropNameMaxLength("Notes", 2048));

        RuleFor(x => x.DateEnd)
            .GreaterThanOrEqualTo(x => x.DateStart)
            .WithMessage("Diet plan end date must be on or after the start date.");

        RuleFor(x => x.Sections)
            .NotNull().WithMessage(CommonErrors.PropNameNotNull("Sections"));

        RuleForEach(x => x.Sections)
            .SetValidator(new DietPlanSectionDtoValidator());
    }

    private sealed class DietPlanSectionDtoValidator : AbstractValidator<DietPlanSectionDto>
    {
        public DietPlanSectionDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Section Title"))
                .MaximumLength(256).WithMessage(CommonErrors.PropNameMaxLength("Section Title", 256));

            RuleFor(x => x.Items)
                .NotNull().WithMessage(CommonErrors.PropNameNotNull("Items"));

            RuleForEach(x => x.Items)
                .SetValidator(new DietPlanSectionItemDtoValidator());
        }
    }

    private sealed class DietPlanSectionItemDtoValidator : AbstractValidator<DietPlanSectionItemDto>
    {
        public DietPlanSectionItemDtoValidator()
        {
            RuleFor(x => x.Label)
                .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Item Label"))
                .MaximumLength(256).WithMessage(CommonErrors.PropNameMaxLength("Item Label", 256));

            RuleFor(x => x.Amount)
                .MaximumLength(128).WithMessage(CommonErrors.PropNameMaxLength("Amount", 128));

            RuleFor(x => x.Note)
                .MaximumLength(1024).When(x => !string.IsNullOrWhiteSpace(x.Note))
                .WithMessage(CommonErrors.PropNameMaxLength("Note", 1024));
        }
    }
}

