namespace VsaSample.Application.Features.SubCategories.Import;

internal sealed class ImportSubCategoriesCommandHandler(
    IApplicationDbContext db,
    IExcelHelper excel) : ICommandHandler<ImportSubCategoriesCommand, int>
{
    public async Task<Result<int>> Handle(ImportSubCategoriesCommand command, CancellationToken cancellationToken)
    {
        var rows = await excel.ImportFromExcelAsync<SubCategoryExcelRow>(command.Content);

        var categories = await db.Categories
            .AsNoTracking()
            .Select(c => new { c.Id, c.Name })
            .ToListAsync(cancellationToken);
        var lookup = categories.ToDictionary(c => c.Name, c => c.Id, StringComparer.OrdinalIgnoreCase);

        var errors = new List<Error>();
        var entities = new List<SubCategory>();

        for (var i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            if (!lookup.TryGetValue(row.CategoryName, out var categoryId))
            {
                errors.Add(CategoryConstants.ExcelImportErrors.CategoryNotFoundByName(row.CategoryName, i + 2));
                continue;
            }

            entities.Add(new SubCategory
            {
                CategoryId = categoryId,
                Name = row.Name,
                IsActive = row.IsActive
            });
        }

        if (errors.Count > 0)
        {
            return Result<int>.ValidationFailure(new ValidationError(errors.ToArray()));
        }

        db.SubCategories.AddRange(entities);
        var saved = await db.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(saved);
    }
}
