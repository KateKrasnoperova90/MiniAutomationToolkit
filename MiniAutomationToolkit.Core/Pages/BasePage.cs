namespace MiniAutomationToolkit.Core.Pages;

public abstract class BasePage
{
    public abstract string Url { get; }
    public abstract string PageName { get; }

    // Здесь общая логика загрузки, одинаковая для всех страниц, — её не нужно дублировать.
    public virtual void Load()
    {
        Console.WriteLine($"Loading page: {PageName} at {Url}");
    }
}