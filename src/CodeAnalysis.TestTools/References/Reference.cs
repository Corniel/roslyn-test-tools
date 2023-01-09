namespace CodeAnalysis.TestTools.References;

/// <summary>A <see cref="MetadataReference"/> helper class.</summary>
public static class Reference
{
    /// <summary>The mscorlib.dll <see cref="MetadataReference"/>.</summary>
    public static readonly MetadataReference Mscorlib = Core("mscorlib.dll");

    /// <summary>The System.dll <see cref="MetadataReference"/>.</summary>
    public static readonly MetadataReference System = Core("System.dll");

    /// <summary>The System.Core.dll <see cref="MetadataReference"/>.</summary>
    public static readonly MetadataReference System_Core = Core("System.Core.dll");

    /// <summary>The System.Globalization.dll <see cref="MetadataReference"/>.</summary>
    public static readonly MetadataReference System_Globalization = Core("System.Globalization.dll");

    /// <summary>The System.Runtime.dll <see cref="MetadataReference"/>.</summary>
    public static readonly MetadataReference System_Runtime = Core("System.Runtime.dll");

    /// <summary>The System.Private.CoreLib.dllSystem.Runtime.dll <see cref="MetadataReference"/>.</summary>
    public static readonly MetadataReference System_Private_CoreLib = Core("System.Private.CoreLib.dll");

    /// <summary>The netstandard.dll <see cref="MetadataReference"/>.</summary>
    public static readonly MetadataReference Netstandard = Core("netstandard.dll");

    /// <summary>The defaults to add.</summary>
    public static readonly MetadataReferences Defaults = new(
        Mscorlib,
        System,
        System_Core,
        System_Globalization,
        System_Runtime,
        System_Private_CoreLib,
        Netstandard);

    /// <summary>Creates a <see cref="MetadataReference"/> based on containing type.</summary>
    public static MetadataReference FromType<TContaining>()
        => MetadataReference.CreateFromFile(typeof(TContaining).Assembly.Location);

    /// <summary>Creates a <see cref="MetadataReference"/> for a file.</summary>
    public static MetadataReference FromFile(params string[] paths) => MetadataReference.CreateFromFile(Path.Combine(paths));

    private static MetadataReference Core(string name) => FromFile(Path.GetDirectoryName(typeof(object).Assembly.Location), name);
}
