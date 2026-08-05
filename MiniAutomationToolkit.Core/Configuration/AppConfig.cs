namespace MiniAutomationToolkit.Core.Configuration;

public class AppConfig
{
    private readonly Dictionary<string, string> _settings = new();

    public AppConfig(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        foreach (string rawLine in lines)
        {
            string line = rawLine.TrimStart();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("#"))
            {
                continue;
            }

            string[] parts = line.Split('=', 2);

            if (parts.Length != 2)
            {
                throw new InvalidDataException($"Invalid configuration line: '{rawLine}'.");
            }

            string key = parts[0].Trim();
            string value = parts[1].Trim();

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidDataException($"Invalid configuration line: '{rawLine}'.");
            }

            if (_settings.ContainsKey(key))
            {
                throw new InvalidDataException($"Duplicate configuration key: '{key}'.");
            }

            _settings[key] = value;
        }
    }

    public T GetSetting<T>(string key)
    {
        if (!_settings.TryGetValue(key, out string? rawValue))
        {
            throw new KeyNotFoundException($"Configuration key '{key}' was not found.");
        }

        try
        {
            return (T)Convert.ChangeType(rawValue, typeof(T));
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            throw new InvalidDataException(
                $"Cannot convert value for key '{key}' to type {typeof(T).Name}.");
        }
    }
}