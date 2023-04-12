using System.Text.Json;

namespace CodeAnalysis.TestTools.References;

/// <summary>Contains NuGet packages and their latest versions.</summary>
[Serializable]
public class NuGetLatestVersions : Dictionary<string, NuGetLatestVersionCheck>
{
    /// <summary>Creates a new instance of the <see cref="NuGetLatestVersions"/> class.</summary>
    public NuGetLatestVersions() { }

    /// <summary>Creates a new instance of the <see cref="NuGetLatestVersions"/> class.</summary>
    protected NuGetLatestVersions(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    /// <summary>Saves the latests versions to a file.</summary>
    public Task SaveAsync(FileInfo file)
    {
        using var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write);
        return JsonSerializer.SerializeAsync(stream, this, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>Saves the latests versions to a stream.</summary>
    public Task SaveAsync(Stream stream)
        => JsonSerializer.SerializeAsync(stream, this);

    /// <summary>Loads the latests versions from a file.</summary>
    [Pure]
    public static async Task<NuGetLatestVersions> LoadAsync(FileInfo file)
    {
        Guard.NotNull(file, nameof(file));
        if (file.Exists)
        {
            using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            return await LoadAsync(stream);
        }
        else return new();
    }

    /// <summary>Loads the latests versions from a stream.</summary>
    [Pure]
    public static Task<NuGetLatestVersions> LoadAsync(Stream stream)
        => JsonSerializer.DeserializeAsync<NuGetLatestVersions>(stream).AsTask()!;
}
