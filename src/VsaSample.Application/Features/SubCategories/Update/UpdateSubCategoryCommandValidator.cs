namespace VsaSample.Application.Features.SubCategories.Update;

internal sealed class UpdateSubCategoryCommandValidator : AbstractValidator<UpdateSubCategoryCommand>
{
    public UpdateSubCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Name"))
            .NotNull();
    }
}

