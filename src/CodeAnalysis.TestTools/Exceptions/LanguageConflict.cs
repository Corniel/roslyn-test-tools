using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CodeAnalysis.TestTools
{
    /// <summary>Thrown when a language conflict occurs.</summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class LanguageConflict : InvalidOperationException
    {
        /// <summary>Creates a new instance of the <see cref="LanguageConflict"/> class.</summary>
        public LanguageConflict()
            : base(Messages.LanguageConflict) { }

        /// <summary>Creates a new instance of the <see cref="LanguageConflict"/> class.</summary>
        public LanguageConflict(string message)
            : base(message) { }

        /// <summary>Creates a new instance of the <see cref="LanguageConflict"/> class.</summary>
        public LanguageConflict(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>Creates a new instance of the <see cref="LanguageConflict"/> class.</summary>
        protected LanguageConflict(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
