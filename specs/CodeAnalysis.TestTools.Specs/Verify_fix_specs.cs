using Specs.CodeFixers;

namespace Verify_fix_specs;

public class Requires
{
    [Test]
    public void any_source()
    {
        Action verify = () => new CSharpOnly().ForCS()
            .ForCodeFix<EmptyFix>()
            .AddSnippet("class SomeClass { }")
            .Verify();

        verify.Should().Throw<IncompleteSetup>()
            .WithMessage("The setup is incomplete. No sources have been configured.");
    }

    [Test]
    public void any_expected_source()
    {
        Action verify = () => new CSharpOnly().ForCS()
            .AddSnippet("class SomeClass { }")
            .ForCodeFix<EmptyFix>()
            .Verify();

        verify.Should().Throw<IncompleteSetup>()
            .WithMessage("The setup is incomplete. No expected sources have been configured.");
    }

    [Test]
    public void singe_source()
    {
        Action verify = () => new CSharpOnly().ForCS()
            .AddSnippet("class SomeClass { }")
            .AddSnippet("class SecondClass { }")
            .ForCodeFix<EmptyFix>()
            .AddSnippet("class OutputClass { }")
            .Verify();

        verify.Should().Throw<NotSupportedException>()
            .WithMessage("A code fix can only be applied on a single source.");
    }

    [Test]
    public void single_expected_source()
    {
        Action verify = () => new CSharpOnly().ForCS()
            .AddSnippet("class InputClass { }")
            .ForCodeFix<EmptyFix>()
            .AddSnippet("class SomeClass { }")
            .AddSnippet("class SecondClass { }")
            .Verify();

        verify.Should().Throw<NotSupportedException>()
            .WithMessage("A code fix can only be verified against a single source.");
    }
}

public class Succeeds_when
{
    [Test]
    public void single_issue_is_fixed()
        => new PreferConstants()
        .ForCS()
        .AddSnippet(@"
class SomeClass
{
    int Answer()
    {
        int n = 42;
        return n;
    }
}
")
        .ForCodeFix<PreferConstantsFix>()
        .AddSnippet(@"
class SomeClass
{
    int Answer()
    {
        const int n = 42;
        return n;
    }
}
")
        .Verify();

    [Test]
    public void multiple_issues_are_fixed()
     => new PreferConstants()
        .ForCS()
        .AddSnippet(@"
class SomeClass
{
    int Answer()
    {
        int n = 42;
        return n;
    }
    public override string ToString()
    {
        int n = 42;
        return n.ToString();
    }
}
")
        .ForCodeFix<PreferConstantsFix>()
        .AddSnippet(@"
class SomeClass
{
    int Answer()
    {
        const int n = 42;
        return n;
    }
    public override string ToString()
    {
        const int n = 42;
        return n.ToString();
    }
}
")
        .Verify();
}

public class Fails_when
{
    [Test]
    public void single_issue_is_fixed()
    {
        Action verify = () => new PreferConstants()
        .ForCS()
        .AddSnippet(@"class SomeClass { }")
        .ForCodeFix<PreferConstantsFix>()
        .AddSnippet(@"class OtherClass { }")
        .Verify();

        verify.Should().Throw<VerificationFailed>()
            .WithMessage(@"The code fix had a different outcome than expected.
Actual code after fix:
class SomeClass { }

Expected code:
class OtherClass { }
");
    }
}
