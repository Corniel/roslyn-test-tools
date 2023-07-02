namespace Microsoft.CodeAnalysis;

/// <summary>Extensions on <see cref="TextDocument"/>.</summary>
public static class TextDocumentExtensions
{
    /// <summary>Represents the <see cref="TextDocument"/> as <see cref="AdditionalText"/>.</summary>
    public static AdditionalText ToAdditionalText(this TextDocument document)
        => new AdditionalTextDocument(document);


    /// <summary>Implements <see cref="AdditionalText"/> for <see cref="TextDocument"/>.</summary>
    private sealed class AdditionalTextDocument : AdditionalText
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

        /// <inheritdoc />
        [Pure]
        public override string ToString() => GetText().ToString();
    }
}
