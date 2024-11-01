namespace Microsoft.CodeAnalysis;

/// <summary>Extensions on <see cref="TextDocument"/>.</summary>
public static class TextDocumentExtensions
{
    /// <summary>Represents the <see cref="TextDocument"/> as <see cref="AdditionalText"/>.</summary>
    [Pure]
    public static AdditionalText ToAdditionalText(this TextDocument document)
        => new AdditionalTextDocument(document);

    /// <summary>Implements <see cref="AdditionalText"/> for <see cref="TextDocument"/>.</summary>
    /// <remarks>Initializes a new instance of the <see cref="AdditionalTextDocument"/> class.</remarks>
    private sealed class AdditionalTextDocument(TextDocument document) : AdditionalText
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TextDocument Document = Guard.NotNull(document);

        /// <inheritdoc/>
        public override string Path => Document.FilePath!;

        /// <inheritdoc/>
        [Pure]
        public override SourceText GetText(CancellationToken cancellationToken = default)
            => Run.Sync(() => Document.GetTextAsync(cancellationToken));

        /// <inheritdoc />
        [Pure]
        public override string ToString() => GetText().ToString();
    }
}
