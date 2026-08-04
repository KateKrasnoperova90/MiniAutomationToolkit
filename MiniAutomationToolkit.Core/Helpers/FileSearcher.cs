namespace MiniAutomationToolkit.Core.Helpers;

public static class FileSearcher
{
    public static string FindFirstScreenshot(List<string> fileNames) // Метод ищет первый файл-скриншот (.png) в переданном списке имён файлов.
    {
        // Any проверяет: есть ли в списке ХОТЯ БЫ ОДИН файл, оканчивающийся на ".png"
        
        bool hasScreenshots = fileNames
            .Any(name => name.EndsWith(".png", StringComparison.OrdinalIgnoreCase)); // OrdinalIgnoreCase нужен, чтобы ".PNG", ".Png" и ".png" считались одинаковыми.

        if (!hasScreenshots)
        {
            throw new FileNotFoundException("No screenshots found in the provided list.");
        }

        // Where отбирает ВСЕ элементы списка, подходящие под условие
        return fileNames
            .Where(name => name.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) 
            .FirstOrDefault()!; // FirstOrDefault берёт из отфильтрованного списка первый элемент.
    }
}