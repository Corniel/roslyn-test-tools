using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeAnalysis.TestTools;

/// <summary>Can load <see cref="Project"/>s using a <see cref="MSBuildWorkspace"/>.</summary>
public static class ProjectLoader
{
    private static readonly object locker = new();

    /// <summary>Loads the project.</summary>
    /// <remarks>
    /// Registers the <see cref="MSBuildLocator.RegisterDefaults()"/>
    /// if no MS build locater has been registered yet.
    /// </remarks>
    public static Project Load(FileInfo location)
    {
        Guard.Exists(location);
        RegisterDefaults();
        return Run.Sync(() => LoadAsync(location));
    }

    private static Task<Project> LoadAsync(FileInfo location)
    {
        using var workspace = MSBuildWorkspace.Create();
        workspace.WorkspaceFailed += OnWorkspaceFailed;
        return workspace.OpenProjectAsync(location.FullName);
    }

    private static void OnWorkspaceFailed(object? sender, WorkspaceDiagnosticEventArgs args)
        => throw new InvalidOperationException(args.Diagnostic.Message);

    private static void RegisterDefaults()
    {
        if (MSBuildLocator.IsRegistered) { return; }

        lock (locker)
        {
            if (!MSBuildLocator.IsRegistered)
            {
                MSBuildLocator.RegisterDefaults();
            }
        }
    }
}
