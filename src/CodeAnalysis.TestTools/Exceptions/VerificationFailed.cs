using System;
using System.Runtime.Serialization;

namespace CodeAnalysis.TestTools
{
    /// <summary>Thrown when the expected issue verification fails.</summary>
    [Serializable]
    public class VerificationFailed : Exception
    {
        /// <summary>Creates a new instance of the <see cref="VerificationFailed"/> class.</summary>
        public VerificationFailed() { }

        /// <summary>Creates a new instance of the <see cref="VerificationFailed"/> class.</summary>
        public VerificationFailed(string message)
            : base(message) { }

        /// <summary>Creates a new instance of the <see cref="VerificationFailed"/> class.</summary>
        public VerificationFailed(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>Creates a new instance of the <see cref="VerificationFailed"/> class.</summary>
        protected VerificationFailed(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
