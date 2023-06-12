using Specs.CodeFixers;

namespace Specs.Rules.PreferConstants_specs;

public class Verifies
{
    [Test]
    public void CSharp()
        => new PreferConstants()
        .ForCS()
        .AddSource(@"Sources\PreferConstants.cs")
        .Verify();
}

public class Fixes
{
    [TestCase(CodeFixKind.Iterative)]
    //[TestCase(CodeFixKind.FixAll)]
    public void CSharp(CodeFixKind fix)
        => new PreferConstants()
        .ForCS()
        .AddSource(@"Sources\PreferConstants.ToFix.cs")
        .ForCodeFix<PreferConstantsFix>()
        .AddSource(@"Sources\PreferConstants.Fixed.cs")
        .Verify(fix);
}

