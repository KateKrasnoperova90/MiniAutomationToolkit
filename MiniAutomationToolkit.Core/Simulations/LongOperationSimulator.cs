using System.Diagnostics;

namespace MiniAutomationToolkit.Core.Simulations;

public class LongOperationSimulator
{
    // Синхронный вариант: Thread.Sleep полностью замораживает поток на 2 секунды.
    public string LongOperation()
    {
        Thread.Sleep(2000);
        return "Done";
    }

    // Асинхронный вариант: Task.Delay освобождает поток на 2 секунды.
    public async Task<string> LongOperationAsync()
    {
        await Task.Delay(2000);
        return "Done";
    }
}