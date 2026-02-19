using System;
using System.IO;
using System.Text.Json;

public class PlaywrightSettings
{
    public string BaseURL { get; set; } = string.Empty;
    public bool Headless { get; set; } = false;
    public AuthConfig Auth { get; set; } = new AuthConfig();

    public static PlaywrightSettings Load(string path = "config.json")
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, path);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Config file not found: {fullPath}");

        var json = File.ReadAllText(fullPath);
        return JsonSerializer.Deserialize<PlaywrightSettings>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}

public class AuthConfig
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
