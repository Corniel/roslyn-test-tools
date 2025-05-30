using System.Security.Cryptography;

namespace CodeAnalysis.TestTools;

/// <summary>Represents a piece of code.</summary>
public sealed class Code
{
    /// <summary>Initializes a new instance of the <see cref="Code"/> class.</summary>
    private Code(string filePath, string text)
    {
        FilePath = filePath;
        Lines = [.. text.Lines()];
    }

    /// <summary>Gets the file name of the code.</summary>
    public string FilePath { get; }

    /// <summary>Gets all lines of the code.</summary>
    public IReadOnlyList<Line> Lines { get; }

    /// <summary>Gets the language of the code.</summary>
    public Language Language => Language.Parse(Path.GetExtension(FilePath));

    /// <inheritdoc />
    [Pure]
    public override string ToString() => string.Join(LineEnd.Unix, Lines);

    /// <summary>Returns true if both pieces of code have the same lines.</summary>
    [Pure]
    public bool HaveSameLines(Code other)
        => Enumerable.SequenceEqual(Lines, Guard.NotNull(other).Lines);

    /// <summary>Creates a code snippet.</summary>
    [Pure]
    public static Code Snippet(string text, Language language)
    {
        Guard.NotNullOrEmpty(text);
        var hash = Convert.ToBase64String(Hash.ComputeHash(Encoding.UTF8.GetBytes(text)))[0..8];
        var filePath = $"Snippet_{hash}{language.FileExtension}";
        return new(filePath, text);
    }

    /// <summary>Reads the code from a file.</summary>
    [Pure]
    public static Code FromFile(FileInfo file)
    {
        Guard.NotNull(file);
        using var reader = new StreamReader(file.OpenRead(), Encoding.UTF8);
        return new(file.ToString(), reader.ReadToEnd());
    }

    /// <summary>Converts a document to code.</summary>
    [Pure]
    public static async Task<Code> FromDocumentAsync(Document document)
    {
        Guard.NotNull(document);
        var path = document.FilePath ?? document.Name;
        return new(path, (await document.GetTextAsync()).ToString());
    }

    private static readonly HashAlgorithm Hash = SHA512.Create();
}
