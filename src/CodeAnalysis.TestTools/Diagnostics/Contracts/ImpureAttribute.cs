namespace CodeAnalysis.TestTools.Diagnostics.Contracts;

/// <summary>To mark a method explicitly as impure.</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
internal class ImpureAttribute : Attribute { }
