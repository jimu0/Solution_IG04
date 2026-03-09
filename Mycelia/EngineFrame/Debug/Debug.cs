using System;

namespace Mycelia.Debug;

public static class Debug
{
    public static void Log(object message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss} {message}");
        Console.ResetColor();
    }
    
    public static void LogWarning(object message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARN] {DateTime.Now:HH:mm:ss} {message}");
        Console.ResetColor();
    }
    
    public static void LogError(object message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {DateTime.Now:HH:mm:ss} {message}");
        Console.ResetColor();
    }
    
    public static void LogSuccess(object message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[SUCCESS] {DateTime.Now:HH:mm:ss} {message}");
        Console.ResetColor();
    }
}
