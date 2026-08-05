namespace MiniAutomationToolkit.Core.Extensions;

public static class StringExtensions
{
    public static bool HasHttpScheme(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        // OrdinalIgnoreCase обеспечивает сравнение без учёта регистра: "HTTPS://" и "https://" будут считаться одинаковыми.
        return input.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || input.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }
}