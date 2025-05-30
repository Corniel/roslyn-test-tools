namespace CodeAnalysis.TestTools.Diagnostics.Contracts;

/// <summary>Indicates the a class is designed to be inheritable.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
[ExcludeFromCodeCoverage(Justification = "Decoration only.")]
internal sealed class InheritableAttribute : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="InheritableAttribute"/> class.</summary>
    public InheritableAttribute(string? message = null) => _ = message;
}
