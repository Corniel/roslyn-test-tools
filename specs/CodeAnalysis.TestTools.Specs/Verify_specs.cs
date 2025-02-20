using Specs;

namespace Verify_specs;

public class Crashes_when
{
    [Test]
    public void analyzer_crashes()
        => Verify.That(() => new CrashingAnalyzer()
            .ForCS()
            .AddSnippet("public class Dummy { }")
            .Verify())
            .Should()
            .Throw<AnalyzerCrashed>()
            .WithMessage("Analyzer 'Specs.Analyzers.CrashingAnalyzer' threw an exception of type *");

    [Test]
    public void no_sources_provided()
        => Verify.That(() => new CSharpOnly()
            .ForCS()
            .Verify())
            .Should().Throw<IncompleteSetup>()
            .WithMessage("The setup is incomplete. No sources have been configured.");
}

public class Succeeds_when
{
    [Test]
    public void Non_precise_issue_is_found_on_same_line()
        => new CSharpOnly()
            .ForCS()
            .AddSnippet("public class MyClass { // Error")
            .Verify();

    [Test]
    public void expected_location_is_matches_actual()
        => new CSharpOnly()
            .ForCS()
            .AddSnippet(@"public class MyClass { // Error ^37#0")
            .Verify();
}

public class Supports
{
    [Test]
    public void Unsafe_CSharp_when_enabled()
        => new CSharpOnly()
            .ForCS()
            .WithUnsafeCode(enable: true)
            .AddSnippet(@"
        public class MyClass 
        {
            unsafe int Risky() => 666;
        }")
            .Verify();

    [Test]
    public void Severity_of_analyzer_other_than_warning()
        => new ReportError()
            .ForCS()
            .AddSnippet(@"
        public class MyClass // Error {{Do not use classes}}
        {
        }")
            .Verify();
}

public class Fails_when
{
    [Test]
    public void Noncompliant_line_raises_error_instead()
        => Verify.That(() => new CSharpOnly()
            .ForCS()
            .AddSnippet("public class MyClass { // Noncompliant")
            .Verify())
            .Should().Throw<VerificationFailed>();

    [Test]
    public void expected_location_is_other_than_actual()
        => Verify.That(() => new CSharpOnly()
            .ForCS()
            .AddSnippet(@"
                    public class MyClass { // Error
                    //     ^^^^")
            .Verify())
        .Should().Throw<VerificationFailed>();
}
