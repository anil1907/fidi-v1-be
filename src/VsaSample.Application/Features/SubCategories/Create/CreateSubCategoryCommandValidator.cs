namespace VsaSample.Application.Features.SubCategories.Create;

internal sealed class CreateSubCategoryCommandValidator : AbstractValidator<CreateSubCategoryCommand>
{
    public CreateSubCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Name"))
            .NotNull();

        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}

