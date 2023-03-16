using PAMSI_1.Logging;

namespace PAMSI_1;

public class Sender
{
    public Sender(Server server)
    {
        _server = server;
    }

    private readonly Server _server;

    private readonly ILogger _logger = new Logger("Sender", LogLevel.Trace);

    public void SendMessage(string message)
    {
        _logger.LogTrace($"Sending message.");

        _server.SendMessage(message);
    }
}
