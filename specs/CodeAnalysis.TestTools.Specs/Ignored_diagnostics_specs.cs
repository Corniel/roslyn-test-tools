namespace Ignored_diagnostics_specs;

public class ForCS
{
    [Test]
    public void Unnecessary_using_directives()
        => new CSharpOnly()
        .ForCS()
        .AddSnippet("using System;")
        .Verify();

    [Test]
    public void Obsolete_code()
       => new CSharpOnly()
       .ForCS()
       .AddSnippet(@"
           class UsingObsolete
           {
               public UsingObsolete()
               {
                   CallObsolete();
               }
               [System.Obsolete]
               void CallObsolete() { }
           }
           ")
       .Verify();
}

public class ForVB
{
    [Test]
    public void Comment_on_multi_line_statement()
        => new VisualBasicOnly()
        .ForVB()
        .AddSnippet(@"
            Class MultiLine
           
                Public Sub New(
                parameter0 as Object, ' Compliant
                parameter1 as Object)
                End Sub
            End Class")
        .WithLanguageVersion(Microsoft.CodeAnalysis.VisualBasic.LanguageVersion.VisualBasic12)
        .Verify();

    [Test]
    public void Obsolete_code()
       => new VisualBasicOnly()
       .ForVB()
       .AddSnippet(@"
           Class UsingObsolete
           
               Public Sub New()
                   CallObsolete()
               End Sub
               <System.Obsolete>
               Sub CallObsolete()
               End Sub
           End Class")
       .Verify();
}
