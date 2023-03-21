namespace PAMSI_1.Logging;

public class Logger : ILogger
{
    public Logger(string source, LogLevel loggerLevel)
    {
        Source = source;
        LoggerLevel = loggerLevel;
    }

    public void Log(LogLevel logLevel, string message)
    {
        if (logLevel < LoggerLevel || LoggerLevel == LogLevel.None) return;

        switch (logLevel)
        {
            case LogLevel.Trace:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                break;

            case LogLevel.Info:
                Console.ForegroundColor = ConsoleColor.White;
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

        Console.WriteLine($"{DateTime.Now,19} [{_levels[(int)logLevel],5}] {Source}: {message}");
        Console.ResetColor();
    }

    private readonly string[] _levels =
    {
        "TRACE", "INFO", "WARN", "ERROR"
    };

    public void LogTrace(string message) => Log(LogLevel.Trace, message);

    public void LogInfo(string message) => Log(LogLevel.Info, message);

    public void LogWarning(string message) => Log(LogLevel.Warning, message);

    public void LogError(string message) => Log(LogLevel.Error, message);

    public string Source { get; set; }
    public LogLevel LoggerLevel { get; set; }
}