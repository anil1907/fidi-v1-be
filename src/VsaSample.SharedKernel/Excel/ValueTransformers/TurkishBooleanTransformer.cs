namespace VsaSample.SharedKernel.Excel.ValueTransformers;

public sealed class TurkishBooleanTransformer : IValueTransformer
{
    public object? Export(object? value)
    {
        if (value is bool b)
        {
            return b ? ExcelConstants.TurkishBooleanTransformer.Yes : ExcelConstants.TurkishBooleanTransformer.No;
        }

        return value;
    }

    public object? Import(object? value)
    {
        if (value is string s)
        {
            if (string.Equals(s, ExcelConstants.TurkishBooleanTransformer.Yes, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(s, ExcelConstants.TurkishBooleanTransformer.No, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return Convert.ChangeType(value!, typeof(bool));
    }
}

