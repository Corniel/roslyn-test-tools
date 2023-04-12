using System.Security.Cryptography;

namespace CodeAnalysis.TestTools;

/// <summary>Represents a piece of code.</summary>
[Inheritable]
public class Code
{
    /// <summary>Creates a new instance of the <see cref="Code"/> class.</summary>
    private Code(string filePath, string text)
    {
        FilePath = filePath;
        Lines = text.Lines().ToArray();
    }

    /// <summary>Gets the file name of the code.</summary>
    public string FilePath { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Line[] Lines;

    /// <summary>Gets the language of the code.</summary>
    public Language Language => Language.Parse(Path.GetExtension(FilePath));

    /// <inheritdoc />
    [Pure]
    public override string ToString() => string.Join(LineEnd.Unix, Lines);

    /// <summary>Creates a code snippet.</summary>
    [Pure]
    public static Code Snippet(string text, Language language)
    {
        Guard.NotNullOrEmpty(text, nameof(text));
        var hash = Convert.ToBase64String(Hash.ComputeHash(Encoding.UTF8.GetBytes(text)))[0..8];
        var filePath = $"Snippet_{hash}{language.FileExtension}";
        return new(filePath, text);
    }

    /// <summary>Reads the code from file.</summary>
    [Pure]
    public static Code FromFile(FileInfo file)
    {
        Guard.NotNull(file, nameof(file));
        using var reader = new StreamReader(file.OpenRead(), Encoding.UTF8);
        return new(file.ToString(), reader.ReadToEnd());
    }

    private static readonly HashAlgorithm Hash = SHA512.Create();
}
