namespace CodeAnalysis.TestTools.Text;

/// <summary>Implements <see cref="AdditionalText"/> for <see cref="TextDocument"/>.</summary>
public sealed class AdditionalTextDocument : AdditionalText
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly TextDocument Document;

    /// <summary>Creates a new instance of the <see cref="AdditionalTextDocument"/> class.</summary>
    public AdditionalTextDocument(TextDocument document)
        => Document = Guard.NotNull(document);

    /// <inheritdoc/>
    public override string Path => Document.FilePath!;

    /// <inheritdoc/>
    [Pure]
    public override SourceText GetText(CancellationToken cancellationToken = default)
        => Run.Sync(() => Document.GetTextAsync(cancellationToken));
}
