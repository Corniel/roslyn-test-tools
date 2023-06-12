using Specs.CodeFixers;

namespace Specs.Rules.PreferConstants_specs;

/// <remarks>
/// This is how tests could look like.
/// </remarks>
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
    [Test]
    public void CSharp()
        => new PreferConstants()
        .ForCS()
        .AddSource(@"Sources\PreferConstants.ToFix.cs")
        .ForCodeFix<PreferConstantsFix>()
        .AddSource(@"Sources\PreferConstants.Fixed.cs")
        .Verify();
}

