using Core;

namespace Infrastructure;

public class ConsoleLogger : ILogger
{
    public void Info(string message)
    {
        WriteLine("[INFO] " + message, ConsoleColor.DarkBlue);
    }

    public void Error(string message)
    {
        WriteLine("[ERROR] " + message, ConsoleColor.DarkRed);
    }

    public void Debug(string message)
    {
        WriteLine("[DEBUG] " + message, ConsoleColor.DarkMagenta);
    }

    private static void WriteLine(string message, ConsoleColor color)
    {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = originalColor;
    }
}