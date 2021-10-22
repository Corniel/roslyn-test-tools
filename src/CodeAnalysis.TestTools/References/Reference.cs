using Microsoft.CodeAnalysis;
using System;
using System.IO;

namespace CodeAnalysis.TestTools.References
{
    /// <summary>A <see cref="MetadataReference"/> helper class.</summary>
    public static class Reference
    {
        /// <summary>Creates a <see cref="MetadataReference"/> based on containing type.</summary>
        public static MetadataReference FromType<TContaining>()
            => MetadataReference.CreateFromFile(typeof(TContaining).Assembly.Location);

        /// <summary>Creates a <see cref="MetadataReference"/> for a file.</summary>
        public static PortableExecutableReference FromFile(string file) => MetadataReference.CreateFromFile(file);
    }
}
