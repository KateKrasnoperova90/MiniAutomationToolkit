namespace MiniAutomationToolkit.Core.Services;

public class ErrorLogger
{
    public string? TryReadFile(string sourceFilePath, string logFilePath)
    {
        try
        {
            // ReadAllText читает весь файл целиком в одну строку, включая переносы строк между строками файла.
            return File.ReadAllText(sourceFilePath);
        }
        catch (Exception ex) when (ex is FileNotFoundException or UnauthorizedAccessException)
        {
            WriteToLog(logFilePath, ex);
            return null;
        }
    }

    private void WriteToLog(string logFilePath, Exception ex)
    {
        string logEntry = $"{DateTime.Now} | {ex.GetType().Name} | {ex.Message}";

        File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
    }
}