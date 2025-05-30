using System.Text.Json;

namespace CodeAnalysis.TestTools.References;

/// <summary>Contains NuGet packages and their latest versions.</summary>
public sealed class NuGetLatestVersions : Dictionary<string, NuGetLatestVersionCheck>
{
    /// <summary>Initializes a new instance of the <see cref="NuGetLatestVersions"/> class.</summary>
    public NuGetLatestVersions() { }

    /// <summary>Saves the latests versions to a file.</summary>
    public Task SaveAsync(FileInfo file)
    {
        using var stream = new FileStream(file.FullName, SaveOptions);
        return JsonSerializer.SerializeAsync(stream, this, JsonOptions);
    }

    /// <summary>Saves the latests versions to a stream.</summary>
    public Task SaveAsync(Stream stream)
        => JsonSerializer.SerializeAsync(stream, this);

    /// <summary>Loads the latests versions from a file.</summary>
    [Pure]
    public static async Task<NuGetLatestVersions> LoadAsync(FileInfo file)
    {
        Guard.NotNull(file);
        if (file.Exists)
        {
            using var stream = new FileStream(file.FullName, LoadOptions);
            try
            {
                return (await JsonSerializer.DeserializeAsync<NuGetLatestVersions>(stream)) ?? [];
            }
            catch (JsonException)
            {
                return [];
            }
        }
        else
        {
            return [];
        }
    }

    private static readonly FileStreamOptions LoadOptions = new()
    {
        Access = FileAccess.Read,
        Mode = FileMode.Open,
        Options = FileOptions.Asynchronous,
    };

    private static readonly FileStreamOptions SaveOptions = new()
    {
        Access = FileAccess.Write,
        Mode = FileMode.Create,
        Options = FileOptions.Asynchronous,
    };

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
    };
}
