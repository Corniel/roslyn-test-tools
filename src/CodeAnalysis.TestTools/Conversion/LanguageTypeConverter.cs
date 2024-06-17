using System.ComponentModel;

namespace CodeAnalysis.TestTools.Conversion;

/// <summary>Implements a <see cref="TypeConverter"/> for <see cref="Language"/>.</summary>
internal sealed class LanguageTypeConverter : TypeConverter
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) => value switch
    {
        null => Language.None,
        string str => Language.Parse(str),
        _ => base.ConvertFrom(context, culture, value),
    };
}
