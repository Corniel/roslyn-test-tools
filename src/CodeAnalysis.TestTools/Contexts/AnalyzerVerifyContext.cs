using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using CodeAnalysis.TestTools.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CodeAnalysis.TestTools.References;

namespace CodeAnalysis.TestTools.Contexts
{
    /// <summary>
    /// Represents the context to verify <see cref="DiagnosticAnalyzer"/> behavior.
    /// </summary>
    public abstract class AnalyzerVerifyContext
    {
        /// <summary>Creates a new instance of the <see cref="AnalyzerVerifyContext"/> class.</summary>
        protected AnalyzerVerifyContext()
        {
            Analyzers = new Analyzers(Language);
            Sources = new Sources(Language);
            References.AddRange(Reference.Defaults);
        }
        
        /// <summary>Gets the language (of the options sources etc.).</summary>
        public abstract Language Language { get; }

        /// <summary>Gets the analyzer(s) to verify for.</summary>
        public Analyzers Analyzers { get; }

        /// <summary>Gets the parse options to compile with.</summary>
        public ParseOptions Options { get; protected set; }
        
        /// <summary>Gets the sources (snippets, files) to verify with.</summary>
        public Sources Sources { get; }

        /// <summary>Gets the diagnostic ID's toe ignore.</summary>
        public DiagnosticIds IgnoredDiagnosics { get; } = new();

        /// <summary>Gets the (external) references to compile with.</summary>
        public MetadataReferences References { get; } = new();

        /// <summary>Gets the output kind of the compilation.</summary>
        public OutputKind OutputKind { get; protected set; } = OutputKind.DynamicallyLinkedLibrary;

        /// <summary>Gets if the compiler warnings should be ignored.</summary>
        public bool IgnoreCompilerWarnings { get; protected set; } = true;

        /// <summary>
        /// Gets the compilation based on the context's:
        /// * sources
        /// * meta-data references
        /// * parse options
        /// * compiler options
        /// </summary>
        public Task<Compilation> GetCompilationAsync()
            => GetProject()
            .WithParseOptions(Options)
            .GetCompilationAsync();

        /// <summary>Reports (both expected, unexpected, and not reported) issues for the analyzer verify context.</summary>
        [DebuggerStepThrough]
        public IEnumerable<Issue> ReportIssues()
            => Run.Sync(() => ReportIssuesAsync());

        /// <summary>Reports (both expected, unexpected, and not reported) issues for the analyzer verify context.</summary>
        public async Task<IEnumerable<Issue>> ReportIssuesAsync()
        {
            var compilation = await GetCompilationAsync();
            var diagnostics = await compilation.GetDiagnosticsAsync(Analyzers);
            var expected = compilation.GetExpectedIssues();

            return IgnoreCompilerWarnings
                ? IssueComparer.Compare(diagnostics.Where(diagnostic => !diagnostic.IsCompilerWarning()), expected, IgnoredDiagnosics)
                : IssueComparer.Compare(diagnostics, expected, IgnoredDiagnosics);
        }

        /// <summary>Updates the compilations options before applying the diagnostics.</summary>
        protected abstract CompilationOptions Update(CompilationOptions options);

        /// <summary>Gets the assembly name of the compilation.</summary>
        protected virtual string AssemblyName => $"{Analyzers.First().GetType().Name}.Verify";
    
        private Project GetProject()
        {
            if (!Sources.Any()) throw new IncompleteSetup(Messages.IncompleteSetup_NoSources);
            else
            {
                using var workspace = new AdhocWorkspace();
                var solution = workspace.CurrentSolution;

                var project = solution
                    .AddProject(AssemblyName, AssemblyName, Language.Name)
                    .AddMetadataReferences(References)
                    .AddSources(Sources);

                return project.WithCompilationOptions(Update(project.CompilationOptions));
            }
        }
    }
}
