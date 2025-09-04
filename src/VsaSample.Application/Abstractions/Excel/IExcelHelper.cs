namespace VsaSample.Application.Abstractions.Excel;

using VsaSample.SharedKernel.Constants;

public interface IExcelHelper
{
    Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName = ExcelConstants.GenericSheetName);
    Task<IReadOnlyList<T>> ImportFromExcelAsync<T>(byte[] content, string sheetName = ExcelConstants.GenericSheetName) where T : new();
}
