namespace VsaSample.SharedKernel.Excel;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelColumnAttribute(string exportName) : Attribute
{
    public string ExportName { get; } = exportName;
    public string? ImportName { get; init; }
    public Type? TransformerType { get; init; }

    public string GetImportName() => ImportName ?? ExportName;
}

