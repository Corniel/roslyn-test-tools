using Microsoft.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Analyzer_verify_context_specs;

public class For_CS_and_VB
{
    [Test]
    public void supports_adding_code_snippets()
    {
        var context = new CSharpAnalyzerVerifyContext()
            .AddSnippet("public class Snippet {}");

        context.Sources.Should().ContainSingle();
    }

    [Test]
    public void supports_adding_source_files()
    {
        var context = new CSharpAnalyzerVerifyContext()
            .AddSource(@"Sources/CodeFile.cs");

        context.Sources.Should().ContainSingle();
    }
}

public class For_CS
{
    [Test]
    public void supports_analyzer_for_multiple_languages()
    {
        Action init = () => _ = new MultipleLanguages().ForCS();
        init.Should().NotThrow();
    }

    [Test]
    public void does_not_support_VB_Analyzer()
    {
        Action init = () => _ = new VisualBasicOnly().ForCS();
        init.Should()
            .Throw<LanguageConflict>()
            .WithMessage("The analyzer 'VisualBasicOnly' does not support C#.");
    }
}
public class For_VB
{
    [Test]
    public void supports_analyzer_for_multiple_languages()
    {
        Action init = () => _ = new MultipleLanguages().ForVB();
        init.Should().NotThrow();
    }
    [Test]
    public void does_not_support_CS_Analyzer()
    {
        Action init = () => _ = new CSharpOnly().ForVB();
        init.Should()
            .Throw<LanguageConflict>()
            .WithMessage("The analyzer 'CSharpOnly' does not support Visual Basic.");
    }
}

public class For_CS_Project
{
    private static readonly FileInfo CSProject = new("../../../../../project/CSharpProject/CSharpProject.csproj");

    [Test]
    public void supports_analyzer_for_multiple_languages()
    {
        Action init = () => _ = new MultipleLanguages().ForProject(CSProject);
        init.Should().NotThrow();
    }

    [Test]
    public void does_not_support_VB_Analyzer()
    {
        Action init = () => _ = new VisualBasicOnly().ForProject(CSProject);
        init.Should()
            .Throw<LanguageConflict>()
            .WithMessage("The analyzer 'VisualBasicOnly' does not support C#.");
    }

    [Test]
    public void supports_CS_Analyzer()
    {
        Action init = () => _ = new CSharpOnly().ForProject(CSProject);
        init.Should().NotThrow();
    }

    [Test]
    public void compiles_with_NuGet_dependency()
        => new CSharpOnly().ForProject(CSProject)
            .ReportIssues()
            .Should().BeEmpty();

    [Test]
    public async Task passes_additional_files()
    {
        var diagnostics = await new CheckAdditionalFiles()
            .ForProject(CSProject)
            .GetDiagnosticsAsync();

        diagnostics
            .Where(d => d.Id == nameof(CheckAdditionalFiles))
            .Select(d => d.GetMessage())
            .Should().BeEquivalentTo("Contains data.txt: 'Hello, world!'");
    }
}

public class For_VB_Project
{
    private static readonly FileInfo VBProject = new("../../../../../project/VbProject/VbProject.vbproj");

    [Test]
    public void supports_analyzer_for_multiple_languages()
    {
        Action init = () => _ = new MultipleLanguages().ForProject(VBProject);
        init.Should().NotThrow();
    }

    [Test]
    public void does_not_support_CS_Analyzer()
    {
        Action init = () => _ = new CSharpOnly().ForProject(VBProject);
        init.Should()
            .Throw<LanguageConflict>()
            .WithMessage("The analyzer 'CSharpOnly' does not support Visual Basic.");
    }

    [Test]
    public void supports_VB_Analyzer()
    {
        Action init = () => _ = new VisualBasicOnly().ForProject(VBProject);
        init.Should().NotThrow();
    }

    [Test]
    public void compiles_with_NuGet_dependency()
        => new VisualBasicOnly().ForProject(VBProject)
            .ReportIssues()
            .Should().BeEmpty();

    [Test]
    public async Task passes_additional_files()
    {
        var diagnostics = await new CheckAdditionalFiles()
            .ForProject(VBProject)
            .GetDiagnosticsAsync();

        diagnostics
            .Where(d => d.Id == nameof(CheckAdditionalFiles))
            .Select(d => d.GetMessage())
            .Should().BeEquivalentTo("Contains data.txt: 'Hello, world!'");
    }
}
