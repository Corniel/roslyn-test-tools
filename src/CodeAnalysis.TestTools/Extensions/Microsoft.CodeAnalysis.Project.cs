namespace Microsoft.CodeAnalysis;

/// <summary>Extensions on <see cref="Project"/>.</summary>
public static class ProjectExtensions
{
    /// <summary>Add sources (as <see cref="Document"/>'s) to the project.</summary>
    [Pure]
    public static Project AddSources(this Project project, IEnumerable<Code> sources)
    {
        Guard.NotNull(project);
        Guard.NotNull(sources);

        foreach (var code in sources)
        {
            project = project.AddDocument(code.FilePath, code.ToString()).Project;
        }
        return project;
    }
}
