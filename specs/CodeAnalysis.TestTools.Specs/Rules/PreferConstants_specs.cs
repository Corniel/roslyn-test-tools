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
