using NuGet.Versioning;

namespace NuGet_packages_specs;

#if RELEASE
    [TestFixture(Ignore = "Not on build")]
#endif
    public class Latest_versions
{
    [Test]
    public void Newtonsoft_Json()
    {
        var package = NuGetPackage.Newtonsoft_Json();
        package.Version.Should().BeGreaterOrEqualTo(new NuGetVersion("13.0.1"));
    }

    [Test]
    public void Microsoft_VisualBasic()
    {
        var package = NuGetPackage.Microsoft_VisualBasic();
        package.Version.Should().BeGreaterOrEqualTo(new NuGetVersion("10.3.0"));
    }
}
