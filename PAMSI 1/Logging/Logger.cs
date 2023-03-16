namespace PAMSI_1.Logging;

public class Logger : ILogger
{
    public Logger(string source, LogLevel logLevel = LogLevel.Warning)
    {
        Source = source;
        LogLevel = logLevel;
    }

    public void Log(LogLevel logLevel, string message)
    {
        if (logLevel < LogLevel) return;

        switch (logLevel)
        {
            case LogLevel.Trace:
                Console.ForegroundColor = ConsoleColor.Gray;
                break;

            case LogLevel.Info:
                Console.ForegroundColor = ConsoleColor.Green;
                break;

            case LogLevel.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;

            case LogLevel.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;

            case LogLevel.None:
                return;

            default:
                Console.ForegroundColor = ConsoleColor.White;
                break;

        }

        Console.WriteLine($"{DateTime.Now,19} [{logLevel,7}] {Source}: {message}");
        Console.ResetColor();
    }

    public void LogTrace(string message) => Log(LogLevel.Trace, message);

    public void LogInfo(string message) => Log(LogLevel.Info, message);

    public void LogWarning(string message) => Log(LogLevel.Warning, message);

    public void LogError(string message) => Log(LogLevel.Error, message);

    public string Source { get; set; }
    public LogLevel LogLevel { get; set; }
}