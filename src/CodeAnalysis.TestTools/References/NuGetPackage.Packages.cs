using NuGet.Versioning;

namespace CodeAnalysis.TestTools.References;

public partial class NuGetPackage
{
    /// <summary>Resolves a package of choice.</summary>
    public static NuGetPackage Resolve(string packageId, string version, string runtime = null)
        => Run.Sync(() => ResolveAsync(packageId, version, runtime));

    /// <summary>Gets the (empty) Microsoft.Build.NoTargets package.</summary>
    public static readonly NuGetPackage Microsoft_Build_NoTargets = new("Microsoft.Build.NoTargets", new NuGetVersion("1.0"), null);

    /// <summary>Gets the FluentAssertions package.</summary>
    public static NuGetPackage FluentAssertions(string version = Latest) => Resolve("FluentAssertions", version);



    /// <summary>Gets the Microsoft.VisualBasic.</summary>
    public static NuGetPackage Microsoft_VisualBasic(string version = Latest) => Resolve("Microsoft.VisualBasic", version);

    /// <summary>Gets the MSTest.TestFramework package.</summary>
    public static NuGetPackage MSTest_TestFramework(string version = Latest) => Resolve("MSTest.TestFramework", version);

    /// <summary>Gets the Newtonsoft.Json package.</summary>
    public static NuGetPackage Newtonsoft_Json(string version = Latest) => Resolve("Newtonsoft.Json", version);

    /// <summary>Gets the NUnit package.</summary>
    public static NuGetPackage NUnit(string version = Latest) => Resolve("NUnit", version);

    /// <summary>Gets the XUnit.Assert package.</summary>
    public static NuGetPackage XUnit_Assert(string version = Latest) => Resolve("xunit.assert", version);

    /// <summary>Gets the Xunit.Extensibility.Core package.</summary>
    public static NuGetPackage Xunit_Extensibility_Core(string version = Latest) => Resolve("xunit.extensibility.core", version);
}
