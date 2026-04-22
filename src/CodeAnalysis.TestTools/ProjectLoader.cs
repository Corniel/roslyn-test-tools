using Buildalyzer;
using Buildalyzer.IO;
using Buildalyzer.Workspaces;

namespace CodeAnalysis.TestTools;

/// <summary>Can load <see cref="Project"/>s using Buildalyzer.</summary>
public static class ProjectLoader
{
    /// <summary>Loads the project.</summary>
    [Pure]
    public static Project Load(FileInfo location)
    {
        var manager = new AnalyzerManager();
        var path = IOPath.Parse(Guard.Exists(location).FullName);
        var analyzer = manager.GetProject(path);
        var workspace = analyzer!.GetWorkspace();
        return workspace.CurrentSolution.Projects.Single();
    }
}
