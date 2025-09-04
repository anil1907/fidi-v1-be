namespace VsaSample.Application.Features.SubCategories.Import;

public sealed record ImportSubCategoriesCommand(byte[] Content) : ICommand<int>;
