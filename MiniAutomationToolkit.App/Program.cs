using System.Diagnostics;
using MiniAutomationToolkit.Core.Helpers;
using MiniAutomationToolkit.Core.Models;
using MiniAutomationToolkit.Core.Services;
using MiniAutomationToolkit.Core.Pages;
using MiniAutomationToolkit.Core.Configuration;
using MiniAutomationToolkit.Core.Extensions;
using MiniAutomationToolkit.Core.Simulations;

PrintStartupMessage();      // Задание 1
RunDiscountCalculator();    // Задание 2
RunScreenshotSearch();      // Задание 3
RunScreenshotSearchWithoutMatches(); // Задание 3
RunUserDtoDemo();           // Задание 4
RunPageObjectDemo();        // Задание 5
RunAppConfigDemo();         // Задание 6
RunStringExtensionsDemo();  // Задание 7
await RunAsyncOperationDemo(); // Задание 8

// ===== Задание 1: сообщение о запуске =====
static void PrintStartupMessage()
{
    Console.WriteLine("MiniAutomationToolkit started");
    Console.WriteLine();
}

// ===== Задание 2: калькулятор скидок с вводом от пользователя =====
static void RunDiscountCalculator()
{
    bool continueCalculating = true;

    while (continueCalculating)
    {
        // Спрашиваем тип клиента (1/2/3). Если выбор некорректный — clientType будет null.
        ClientType? clientType = AskClientType();
        if (clientType is null)
        {
            Console.WriteLine("Error: invalid client type selected.");
        }
        else
        {
            // Спрашиваем сумму заказа. Если введено не число — amount будет null.
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
                    // Сработает, если сумма отрицательная.
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        Console.WriteLine();
        continueCalculating = AskToContinue();
        Console.WriteLine();
    }
}

// Печатает меню выбора клиента и возвращает выбор пользователя

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

// Просит ввести сумму заказа.
static double? AskOrderAmount()
{
    Console.Write("Enter order amount: ");
    string? input = Console.ReadLine();

    return double.TryParse(input, out double amount) ? amount : null;
}

// Спрашивает, продолжать ли цикл расчётов.
static bool AskToContinue()
{
    Console.Write("Check another discount? (y/n): ");
    string? input = Console.ReadLine();

    return string.Equals(input, "y", StringComparison.OrdinalIgnoreCase);
}

// ===== Задание 3: поиск первого скриншота в списке файлов =====
static void RunScreenshotSearch()
{
    List<string> fileNames =
    [
        "screen_001.ng",
        "error_2024.log",
        "screen_002.png",
        "debug.txt",
        "report_final.PNG",
        "session.log",
        "notes.txt",
        "screen_003.png",
        "trace_007.log",
        "readme.txt",
        "capture_01.png",
        "output.log",
        "summary.txt",
        "screen_004.png",
        "warning.log",
        "checklist.txt",
        "screen_005.png",
        "audit.log",
        "config.txt",
        "screen_006.png"
    ];

    string firstScreenshot = FileSearcher.FindFirstScreenshot(fileNames);
    Console.WriteLine($"First screenshot found: {firstScreenshot}");
    Console.WriteLine();
}

// ===== Задание 3: демонстрация обработки ошибки, когда скриншотов нет =====
static void RunScreenshotSearchWithoutMatches()
{
    List<string> fileNamesWithoutScreenshots =
    [
        "error_2024.log",
        "debug.txt",
        "session.log",
        "notes.txt",
        "trace_007.log",
        "readme.txt"
    ];

    try
    {
        FileSearcher.FindFirstScreenshot(fileNamesWithoutScreenshots);
    }
    catch (FileNotFoundException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

// ===== Задание 4: неизменяемая модель пользователя =====
static void RunUserDtoDemo()
{
    // --- Успешное создание ---
    UserDto user = new UserDto("Alex Smith", "alex@example.com");
    Console.WriteLine($"User created: {user.Name}, {user.Email}");

    // --- Равенство по значению ---
    UserDto sameUser = new UserDto("Alex Smith", "alex@example.com");
    Console.WriteLine($"Objects are equal: {user == sameUser}");

    // --- Неизменяемость ---
    UserDto modifiedUser = user with { };
    Console.WriteLine($"Original user is unchanged: {user.Name}, {user.Email}");
    Console.WriteLine();

    // --- Некорректные данные ---
    TryCreateUser("", "alex@example.com");            // пустое имя
    TryCreateUser("Alex Smith", "");                  // пустой email
    TryCreateUser("Alex Smith", "alex.example.com");  // нет символа @
    TryCreateUser("Alex Smith", "alex @example.com"); // пробел в email
    Console.WriteLine();
}

// Вспомогательный метод: пытается создать пользователя и печатает сообщение об ошибке, если данные некорректны.
static void TryCreateUser(string name, string email)
{
    try
    {
        UserDto user = new UserDto(name, email);
        Console.WriteLine($"User created: {user.Name}, {user.Email}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

// ===== Задание 5: базовая страница и наследники =====
static void RunPageObjectDemo()
{
    List<BasePage> pages =
    [
        new LoginPage(),
        new HomePage()
    ];

    foreach (BasePage page in pages)
    {
        page.Load();
    }

    Console.WriteLine();
    CheckUrlsAreUnique(pages);

    // --- Демонстрация ситуации с дубликатом ---
    // Добавляем ещё одну LoginPage: её Url "/login" повторится.
    List<BasePage> pagesWithDuplicate =
    [
        new LoginPage(),
        new HomePage(),
        new LoginPage()
    ];

    try
    {
        CheckUrlsAreUnique(pagesWithDuplicate);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Console.WriteLine();
}

// Проверяет, что среди страниц нет двух с одинаковым Url.
static void CheckUrlsAreUnique(List<BasePage> pages)
{
    // Select достаёт из каждого объекта только его Url — получается список адресов.
    // Distinct убирает повторы: если два адреса совпали, останется один.
    int uniqueUrlCount = pages
        .Select(page => page.Url)
        .Distinct()
        .Count();

    // Если после удаления повторов элементов стало меньше — значит, дубликаты были.
    if (uniqueUrlCount != pages.Count)
    {
        throw new InvalidOperationException("Duplicate page URLs found.");
    }

    Console.WriteLine("All page URLs are unique");
}

// ===== Задание 6: чтение настроек из текстового файла =====
static void RunAppConfigDemo()
{
    string configPath = Path.Combine(AppContext.BaseDirectory, "data", "appsettings.txt");

    AppConfig config = new AppConfig(configPath);

    // Для каждого параметра запрашиваем СВОЙ тип
    string baseUrl = config.GetSetting<string>("baseUrl");
    int timeout = config.GetSetting<int>("timeout");
    bool headless = config.GetSetting<bool>("headless");
    int retryCount = config.GetSetting<int>("retryCount");

    Console.WriteLine($"baseUrl: {baseUrl}");
    Console.WriteLine($"timeout: {timeout}");
    Console.WriteLine($"headless: {headless}");
    Console.WriteLine($"retryCount: {retryCount}");
    Console.WriteLine();

    // --- Демонстрация отсутствующего ключа ---
    try
    {
        config.GetSetting<string>("missingKey");
    }
    catch (KeyNotFoundException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Console.WriteLine();
}

// ===== Задание 7: метод расширения для строк =====
static void RunStringExtensionsDemo()
{
    string?[] inputs =
    [
        "https://google.com",
        "http://example.org",
        "ftp://files.example.com",
        null,
        "HTTPS://SITE.EXAMPLE.COM"
    ];

    foreach (string? input in inputs)
    {
        bool result = input.HasHttpScheme();

        Console.WriteLine($"{input ?? "<null>"} → {result}");
    }

    Console.WriteLine();
}

// ===== Задание 8: асинхронная длительная операция =====

static async Task RunAsyncOperationDemo()
{
    LongOperationSimulator simulator = new LongOperationSimulator();

    // Stopwatch — встроенный секундомер для измерения времени выполнения.
    // StartNew сразу создаёт объект и запускает отсчёт.
    Stopwatch stopwatch = Stopwatch.StartNew();

    // await приостанавливает выполнение этого метода до готовности результата, но НЕ блокирует поток — в этом ключевое отличие от .Result и .Wait().
    string result = await simulator.LongOperationAsync();

    // Stop останавливает секундомер, дальше читаем накопленное время.
    stopwatch.Stop();

    Console.WriteLine($"Async result: {result}");
    Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds} ms");
    Console.WriteLine();
}