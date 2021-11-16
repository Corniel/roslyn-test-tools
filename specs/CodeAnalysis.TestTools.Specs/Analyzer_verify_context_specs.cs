namespace Analyzer_verify_context_specs;

public class For_both
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

public class ForCS
{
    [Test]
    public void supports_analyzer_for_multiple_languages()
    {
        Action init = () => new MultipleLanguages().ForCS();
        init.Should().NotThrow<LanguageConflict>();
    }

    [Test]
    public void does_not_support_VB_Analyzer()
    {
        Action init = () => new VisualBasicOnly().ForCS();
        init.Should()
            .Throw<LanguageConflict>()
            .WithMessage("The analyzer 'VisualBasicOnly' does not support C#.");
    }
}
public class ForVB
{
    [Test]
    public void supports_analyzer_for_multiple_languages()
    {
        Action init = () => new MultipleLanguages().ForVB();
        init.Should().NotThrow<LanguageConflict>();
    }
    [Test]
    public void does_not_support_CS_Analyzer()
    {
        Action init = () => new CSharpOnly().ForVB();
        init.Should()
            .Throw<LanguageConflict>()
            .WithMessage("The analyzer 'CSharpOnly' does not support Visual Basic.");
    }
}
