namespace PAMSI_1.Logging;

public interface ILogger
{
    void Log(LogLevel logLevel, string message);

    void LogTrace(string message);

    void LogInfo(string message);

    void LogWarning(string message);

    void LogError(string message);

    string Source { get; set; }

    LogLevel LogLevel { get; set; }
}

public enum LogLevel
{
    Trace,
    Info,
    Warning,
    Error,
    None
}