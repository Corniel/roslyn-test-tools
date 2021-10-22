# Roslyn (Static) Code Analysis Test Tools
The package provides tooling to verify the behavior of a [Diagnostic Analyzer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.diagnostics.diagnosticanalyzer).
It does this by providing a (test) context builder, and a mechanism to define
expected issues in code (files).

THe idea of defining expected issues that way, has been inspired by
[SonarSource](https://sonarsource.com). The same patterns are used, and their
[implementation](https://github.com/SonarSource/sonar-dotnet) has been an
inspiration for this one.

## Defining expected issues
The basic idea is that by adding comments (of a certain format) to a code file
the verifier compare the actual raised issues and errors to the expected onces.
If there are differences, the verification fails.

The easiest way to this, is adding `// Noncompliant` to the end of a line
that contains an issue:
``` C#
class MyClass { } // Noncompliant
```

By adding also `{{Some expected message}}` the message itself is also checked:

``` C#
class MyClass { } // Noncompliant {{Visibility should be specified.}}
```

To check the text span that is been reported on, two different options are
supported:

``` C#
namespace MyNameSpace
{
    class MyClass { } // Noncompliant {{Visibility should be specified.}}
//  ^^^^^^^^^^^^^
}
```

Or, Alternatively:
``` C#
namespace MyNameSpace
{
    class MyClass { } // Noncompliant ^4#12 {{Visibility should be specified.}}
}
```

If wanted, also the expected diagnostic ID can be specified:
``` C#
class MyClass { } // Noncompliant [IDE0005] {{Visibility should be specified.}}
```

or multiple:
``` C#
class MyClass { } // Noncompliant [IDE0005,XS034]
```

In some cases, it makes more sense to put the comment on another line than on the
line of the issue. In that case, a (line) offset can be provided:

``` C#
// Noncompliant@+1 [IDE0005,XS034]
class MyClass { }
```

### Expected errors
Instead of a line of code being noncompliant, it is possible to mark a line as
an error too. Just use `Error` instead of `Noncompliant`:

``` C#
public class Missing { // Error [CS1513]
```

## Setup a verification test
A test can be setup up justing a test framework of choice. The examples here
use NUnit.

``` C#
[Test]
public void Verify_MyCSharpAnalyzer()
    => new Verify_MyCSharpAnalyzer()
        .ForCS()
        // or use .AddSnippet() where you provide the code as string.
        .AddSource("Path\to\testcode.cs")
        .Verify();

[Test]
public void Verify_MyVBAnalyzer()
    => new Verify_MyVBAnalyzer()
        .ForVB()
        .AddSource("Path\to\testcode.vb")
        .Verify();
```

This will run your analyzer on the provide code, and throw a
`VerificationFailed` exception if any expected issue did not raise or if any
unexpected issue (or error) occurred.

By default the following external references are included:
 * `System.Data.Common`
 * `System.Linq`
 * `System.Runtime`
 * `System.Globalization`
 * `Microsoft.VisualBasic` (Visual Basic only)
 
The code base is compiled against the latest language version (currently C# 9,
and Visual Basic 16.9).

### Tweaking the Verify Analyzer Context
The initialized context has been for one language. All changes/additions take
that in to consideration. So extra analyzers/source files, etcetera, must be
supported by that language.

### Additional analyzers
Extra analyzers can be added using `.Add(iagnosticAnalyzer analyzer)`.

### Adding extra source code
Extra files can be added using `.AddSource(string path)`.
Extra code snippets can be added using `.AddSnippet(string snippet)`.

### Adding external references
Extra references can be added using `.AddReference<TContainingType>()`,
where the assembly of `TContainingType` is added. Alternatively 
`.AddReferences(params MetadataReference[] references)` and
`.AddPackages(params NuGetPackage[] packages)` can be used.

### Set Output kind
The output kind of the assembly built (default `OutputKind.DynamicallyLinkedLibrary`)
can be changed using `.WithOutputKind(OutputKind outputKind)`.

### Set Language Version/Parse Options
The easiest way to change the language version (and with that the parsings
options) is by using `.WithLanguageVersion(LanguageVersion version)`.
Alternatively `.WithOptions(ParseOptions options)` can be used.
The arguments are based on the context of choice (C# or Visual Basic), so
you can not change the language with these methods.

### Ignore specific warnings/errors
Some (build) errors and warnings should/could be ignored. This can be done by
using `.WithIgnoredDiagnostics(params string[] diagnosticIds)`.

### Allow unsafe code (C# only)
If desired, `AllowUnsafe` can be set to `true` by using `.WithUnsafeCode(true)`.

### Set Global Imports (Visual Basic only)
The global imports can be changed with `.WithGlobalImports(IEnumerable<GlobalImport> imports)`.
By default these imports are done globally:
``` Visual Basic
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports System.Linq
Imports System.Threading.Tasks
```

