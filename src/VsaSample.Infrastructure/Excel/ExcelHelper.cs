using System.Data;
using ClosedXML.Excel;
using VsaSample.SharedKernel.Excel;

namespace VsaSample.Infrastructure.Excel;

public sealed class ExcelHelper : IExcelHelper
{
    public async Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName = ExcelConstants.GenericSheetName)
    {
        return await Task.Run(() =>
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);
            var table = ToDataTable(data);
            worksheet.Cell(1, 1).InsertTable(table);
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        });
    }

    public async Task<IReadOnlyList<T>> ImportFromExcelAsync<T>(byte[] content, string sheetName = ExcelConstants.GenericSheetName) where T : new()
    {
        return await Task.Run(() =>
        {
            using var stream = new MemoryStream(content);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(sheetName);
            var rows = new List<T>();

            var props = typeof(T).GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(ExcelIgnoreAttribute)))
                .ToList();

            var headers = worksheet.Row(1).Cells().Select(c => c.GetString()).ToList();
            foreach (var dataRow in worksheet.RowsUsed().Skip(1))
            {
                var item = new T();
                foreach (var prop in props)
                {
                    var attr = prop.GetCustomAttributes(typeof(ExcelColumnAttribute), false)
                        .Cast<ExcelColumnAttribute>()
                        .FirstOrDefault();
                    var columnName = attr?.GetImportName() ?? prop.Name;
                    var index = headers.FindIndex(h => h == columnName);
                    if (index < 0) continue;
                    var cell = dataRow.Cell(index + 1);
                    object? value = cell.Value;
                    if (attr?.TransformerType is not null &&
                        Activator.CreateInstance(attr.TransformerType) is IValueTransformer transformer)
                    {
                        value = transformer.Import(value);
                    }
                    else
                    {
                        var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        value = Convert.ChangeType(value, targetType);
                    }
                    prop.SetValue(item, value);
                }
                rows.Add(item);
            }

            return rows;
        });
    }

    private static DataTable ToDataTable<T>(IEnumerable<T> data)
    {
        var dt = new DataTable(typeof(T).Name);
        var props = typeof(T).GetProperties()
            .Where(p => !Attribute.IsDefined(p, typeof(ExcelIgnoreAttribute)))
            .ToList();

        foreach (var prop in props)
        {
            var attr = prop.GetCustomAttributes(typeof(ExcelColumnAttribute), false)
                .Cast<ExcelColumnAttribute>()
                .FirstOrDefault();
            var columnName = attr?.ExportName ?? prop.Name;
            dt.Columns.Add(columnName);
        }

        foreach (var item in data)
        {
            var row = dt.NewRow();
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes(typeof(ExcelColumnAttribute), false)
                    .Cast<ExcelColumnAttribute>()
                    .FirstOrDefault();
                var columnName = attr?.ExportName ?? prop.Name;
                var value = prop.GetValue(item);
                if (attr?.TransformerType is not null &&
                    Activator.CreateInstance(attr.TransformerType) is IValueTransformer transformer)
                {
                    value = transformer.Export(value);
                }
                row[columnName] = value ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }

        return dt;
    }
}
