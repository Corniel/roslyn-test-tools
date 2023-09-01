using Buildalyzer;
using Buildalyzer.Workspaces;

namespace CodeAnalysis.TestTools;

/// <summary>Can load <see cref="Project"/>s using Buildalyzer.</summary>
public static class ProjectLoader
{
    /// <summary>Loads the project.</summary>
    public static Project Load(FileInfo location)
    {
        var manager = new AnalyzerManager();
        var analyzer = manager.GetProject(Guard.Exists(location).FullName);
        var workspace = analyzer.GetWorkspace();
        return workspace.CurrentSolution.Projects.Single();
    }
}
