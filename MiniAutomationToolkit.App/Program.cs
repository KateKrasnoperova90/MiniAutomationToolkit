using MiniAutomationToolkit.Core.Models;
using MiniAutomationToolkit.Core.Services;

bool continueCalculating = true;

while (continueCalculating)
{
    ClientType? clientType = AskClientType();
    if (clientType is null)
    {
        Console.WriteLine("Error: invalid client type selected.");
    }
    else
    {
        double? amount = AskOrderAmount();
        if (amount is null)
        {
            Console.WriteLine("Error: entered value is not a valid number.");
        }
        else
        {
            try
            {
                double discount = DiscountCalculator.CalculateDiscount(amount.Value, clientType.Value);
                Console.WriteLine(
                    $"Client: {clientType}, amount: {amount:0.##}, discount: {discount:0.##}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    Console.WriteLine();
    continueCalculating = AskToContinue();
    Console.WriteLine();
}

static ClientType? AskClientType()
{
    Console.WriteLine("Select client type:");
    Console.WriteLine("1 - Regular");
    Console.WriteLine("2 - Premium");
    Console.WriteLine("3 - Vip");
    Console.Write("Your choice: ");

    return Console.ReadLine() switch
    {
        "1" => ClientType.Regular,
        "2" => ClientType.Premium,
        "3" => ClientType.Vip,
        _ => null
    };
}

static double? AskOrderAmount()
{
    Console.Write("Enter order amount: ");
    string? input = Console.ReadLine();

    return double.TryParse(input, out double amount) ? amount : null;
}

static bool AskToContinue()
{
    Console.Write("Check another discount? (y/n): ");
    string? input = Console.ReadLine();

    return string.Equals(input, "y", StringComparison.OrdinalIgnoreCase);
}
