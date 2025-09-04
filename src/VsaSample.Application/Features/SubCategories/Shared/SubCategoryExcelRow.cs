namespace VsaSample.Application.Features.SubCategories.Shared;

using VsaSample.SharedKernel.Excel;
using VsaSample.SharedKernel.Excel.ValueTransformers;

public sealed record class SubCategoryExcelRow
{
    [ExcelColumn(SubCategoryConstants.Excel.Columns.CategoryName)]
    public string CategoryName { get; set; } = default!;

    [ExcelColumn(SubCategoryConstants.Excel.Columns.Name)]
    public string Name { get; set; } = default!;

    [ExcelColumn(SubCategoryConstants.Excel.Columns.Status, TransformerType = typeof(TurkishBooleanTransformer))]
    public bool IsActive { get; set; }

    public SubCategoryExcelRow()
    {
    }

    public SubCategoryExcelRow(string categoryName, string name, bool isActive)
    {
        CategoryName = categoryName;
        Name = name;
        IsActive = isActive;
    }
}

