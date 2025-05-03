using Discord;

namespace Frombot.Services;


public static class Logger
{
    public enum LogLevel { Info, Warning, Error }

    public static Task Log(string message, LogLevel level = LogLevel.Info)
    {
        string timestamp = $"[{DateTime.Now:HH:mm:ss}]";
        string levelTag = $"[{level.ToString().ToUpper()}]";

        Console.WriteLine($"{timestamp} {levelTag}: {message}");
        return Task.CompletedTask;
    }

    public static Task Log(LogMessage log)
    {
        Console.WriteLine($"{log}");
        return Task.CompletedTask;
    }
}
