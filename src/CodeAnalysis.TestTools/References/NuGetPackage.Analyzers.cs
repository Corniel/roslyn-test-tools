using System.Reflection;

namespace CodeAnalysis.TestTools.References;

public partial class NuGetPackage
{
    /// <inheritdoc cref="ResolveAnalyzersAsync(string, Language, string?)" />
    [Pure]
    public static Analyzers ResolveAnalyzers(string packageId, Language language, string? version = Latest)
        => Run.Sync(() => ResolveAnalyzersAsync(packageId, language, version));

    /// <summary>Resolves an analyzers package of choice.</summary>
    [Pure]
    public static async Task<Analyzers> ResolveAnalyzersAsync(string packageId, Language language, string? version = Latest)
    {
        var analyzers = new Analyzers(language);

        var folders = await ResolveFoldersAsync(packageId, version: version);

        var assemblies = new List<Assembly>();

        foreach (var dll in folders
            .SelectMany(f => f.Files)
            .Where(f => f.Extension == ".dll"))
        {
            using var stream = new MemoryStream();

            await dll.OpenRead().CopyToAsync(stream);
            assemblies.Add(Assembly.Load(stream.ToArray()));
        }

        using (new AssemblyResolver(assemblies))
        {
            foreach (var assembly in assemblies)
            {
                var additions = assembly.GetTypes()
                    .Where(tp => IsDiagnosticAnalyzer(tp, language))
                    .Select(Activator.CreateInstance)
                    .OfType<DiagnosticAnalyzer>();

                analyzers = analyzers.AddRange(additions);
            }
        }

        return analyzers;
    }

    [Pure]
    private static bool IsDiagnosticAnalyzer(Type type, Language language)
        => !type.IsAbstract
        && type.IsAssignableTo(typeof(DiagnosticAnalyzer))
        && type.GetCustomAttribute<DiagnosticAnalyzerAttribute>() is { } attr
        && attr.Languages.Contains(language.Name);

    private sealed class AssemblyResolver : IDisposable
    {
        private readonly IReadOnlyCollection<Assembly> Assemblies;

        public AssemblyResolver(IReadOnlyCollection<Assembly> assemblies)
        {
            Assemblies = assemblies;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        [Pure]
        private Assembly? ResolveAssembly(object? sender, ResolveEventArgs args)
            => Assemblies.FirstOrDefault(x => x.FullName == args.Name);

        public void Dispose()
            => AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssembly;
    }
}
