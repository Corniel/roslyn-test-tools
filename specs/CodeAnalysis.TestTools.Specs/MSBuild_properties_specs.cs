using Specs.Analyzers;

namespace CheckBuildProperty_specs;

public class MSBuild_properties
{
    [Test]
    public async Task not_passed_when_not_configured()
    {
        var diagnostics = await new CheckBuildProperty("TestProp")
            .ForCS()
            .AddSnippet("public class Snippet { }")
            .GetDiagnosticsAsync();

        diagnostics
            .Where(d => d.Id == nameof(CheckBuildProperty))
            .Should().BeEmpty();
    }

    [Test]
    public async Task passed_as_compiler_visible_property()
    {
        var diagnostics = await new CheckBuildProperty("TestProp")
            .ForCS()
            .WithBuildProperty("TestProp", "42")
            .AddSnippet("public class Snippet { }")
            .GetDiagnosticsAsync();

        diagnostics
            .Where(d => d.Id == nameof(CheckBuildProperty))
            .Select(d => d.GetMessage())
            .Should().BeEquivalentTo("TestProp = '42'");
    }

    [Test]
    public async Task passed_along_when_using_project_context()
    {
        var diagnostics = await new CheckBuildProperty("TestProp")
            .ForProject(new("../../../../../projects/CSharpProject/CSharpProject.csproj"))
            .WithBuildProperty("TestProp", "true")
            .GetDiagnosticsAsync();

        diagnostics
            .Where(d => d.Id == nameof(CheckBuildProperty))
            .Should().ContainSingle();
    }
}
