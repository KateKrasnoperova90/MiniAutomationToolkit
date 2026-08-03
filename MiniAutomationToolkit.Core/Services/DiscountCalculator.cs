using MiniAutomationToolkit.Core.Models;

namespace MiniAutomationToolkit.Core.Services;

public static class DiscountCalculator
{
    public static double CalculateDiscount(double orderAmount, ClientType clientType)
    {
        if (orderAmount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(orderAmount),
                orderAmount,
                "Order amount cannot be negative.");
        }

        double discountRate = clientType switch
        {
            ClientType.Vip => 0.15,
            ClientType.Premium when orderAmount > 1000 => 0.10,
            ClientType.Premium => 0.05,
            ClientType.Regular when orderAmount > 1000 => 0.05,
            ClientType.Regular => 0,
            _ => throw new ArgumentOutOfRangeException(
                nameof(clientType),
                clientType,
                "Unknown client type.")
        };

        return orderAmount * discountRate;
    }
}