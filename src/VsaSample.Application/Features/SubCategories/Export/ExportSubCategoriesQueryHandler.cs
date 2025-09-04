namespace VsaSample.Application.Features.SubCategories.Export;

internal sealed class ExportSubCategoriesQueryHandler(
    IApplicationDbContext db,
    IExcelHelper excel) : IQueryHandler<ExportSubCategoriesQuery, byte[]>
{
    public async Task<Result<byte[]>> Handle(ExportSubCategoriesQuery query, CancellationToken cancellationToken)
    {
        var items = await db.SubCategories
            .AsNoTracking()
            .Select(e => new SubCategoryExcelRow(e.Category.Name, e.Name, e.IsActive))
            .ToListAsync(cancellationToken);
        var content = await excel.ExportToExcelAsync(items);
        return Result<byte[]>.Success(content);
    }
}
