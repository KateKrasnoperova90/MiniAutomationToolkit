using MiniAutomationToolkit.Core.Models;

namespace MiniAutomationToolkit.Core.Repositories;

public static class ProductRepository
{
    public static List<Product> LoadFromCsv(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        List<Product> products = new List<Product>();

        // Начинаем с i = 1, потому что строка с индексом 0 — это заголовок
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            int lineNumber = i + 1;

            products.Add(ParseLine(line, lineNumber));
        }

        return products;
    }

    // Приватный метод: разбирает одну строку CSV в объект Product.
    private static Product ParseLine(string line, int lineNumber)
    {
        string[] parts = line.Split(';');

        // Должно быть ровно три поля: название, цена, категория.
        if (parts.Length != 3)
        {
            throw new InvalidDataException($"Invalid CSV line {lineNumber}: '{line}'.");
        }

        string name = parts[0].Trim();
        string priceText = parts[1].Trim();
        string categoryText = parts[2].Trim();

        // Ни одно поле не должно быть пустым.
        if (string.IsNullOrWhiteSpace(name)
            || string.IsNullOrWhiteSpace(priceText)
            || string.IsNullOrWhiteSpace(categoryText))
        {
            throw new InvalidDataException($"Invalid CSV line {lineNumber}: '{line}'.");
        }

        if (!decimal.TryParse(
                priceText,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal price))
        {
            throw new InvalidDataException($"Invalid CSV line {lineNumber}: '{line}'.");
        }

        if (price < 0)
        {
            throw new InvalidDataException($"Invalid CSV line {lineNumber}: '{line}'.");
        }

        // Enum.TryParse превращает строку в значение перечисления.
        if (!Enum.TryParse(categoryText, true, out ProductCategory category))
        {
            throw new InvalidDataException($"Invalid CSV line {lineNumber}: '{line}'.");
        }

        return new Product(name, price, category);
    }

    public static List<string> GetAffordableProducts(
        IEnumerable<Product> products,
        ProductCategory category,
        decimal maxPrice)
    {
        return products
            .Where(product => product.Category == category)   // только нужная категория
            .Where(product => product.Price < maxPrice)       // строго дешевле лимита
            .OrderBy(product => product.Price)                // сначала по цене
            .ThenBy(product => product.Name)                  // при равной цене — по названию
            .Select(product => product.Name)                  // берём только название
            .ToList();                                       // превращаем в List<string>
    }
}