namespace CodeAnalysis.TestTools;

/// <summary>Represents a (code) language.</summary>
public readonly struct Language : IEquatable<Language>
{
    /// <summary>None.</summary>
    public static readonly Language None;

    /// <summary>C#.</summary>
    public static readonly Language CSharp = new(CS);

    /// <summary>Visual Basic.</summary>
    public static readonly Language VisualBasic = new(VB);

    /// <summary>F#.</summary>
    public static readonly Language FSharp = new(FS);

    /// <summary>XML.</summary>
    public static readonly Language XML = new(XM);

    /// <summary>Creates a new instance of the <see cref="Language"/> struct.</summary>
    private Language(int lang) => code = lang;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly int code;

    /// <summary>Gets the name of language.</summary>
    public string Name
        => code switch
        {
            00 => string.Empty,
            CS => LanguageNames.CSharp,
            VB => LanguageNames.VisualBasic,
            FS => LanguageNames.FSharp,
            XM => nameof(XML),
            _ => "?",
        };

    /// <summary>Gets the (common) file extension of the language.</summary>
    public string FileExtension
       => code switch
       {
           00 => string.Empty,
           CS => ".cs",
           VB => ".vb",
           FS => ".fs",
           XM => ".xml",
           _ => "?",
       };

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Name;

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object obj) => obj is Language other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(Language other) => code == other.code;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => code;

    /// <summary>Returns true if the languages are the same.</summary>
    public static bool operator ==(Language l, Language r) => l.Equals(r);

    /// <summary>Returns false if the languages are the same.</summary>
    public static bool operator !=(Language l, Language r) => !(l == r);

    /// <summary>parses the language.</summary>
    [Pure]
    public static Language Parse(string str)
        => str?.ToUpperInvariant().Replace(" ", "") switch
        {
            "" or null => None,
            "CSHARP" or "CS" or "C#" or ".CS" => CSharp,
            "VB" or "VBNET" or "VISUALBASIC" or ".VB" => VisualBasic,
            "FSHARP" or "FS" or "F#" or ".FS" => FSharp,
            "XML" or "EXTENSIBLEMARKUPLANGUAGE" or ".XML" => FSharp,
            _ => throw new FormatException(Messages.Language_InvalidFormat),
        };


    private const int CS = 1;
    private const int VB = 2;
    private const int FS = 3;
    private const int XM = 4;
}
