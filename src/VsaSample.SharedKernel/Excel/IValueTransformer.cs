namespace VsaSample.SharedKernel.Excel;

public interface IValueTransformer
{
    object? Export(object? value);
    object? Import(object? value);
}

