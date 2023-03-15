namespace PAMSI_1;

public class Sender
{
    public Sender(Server server)
    {
        _server = server;
    }

    private readonly Server _server;
    public required int PacketLength { get; init; }

    public void SendMessage(string message)
    {
        var transmission = new OutgoingTransmission(message, PacketLength);

        Console.WriteLine("Sending transmission");
        
        _server.SendMessage(transmission);
    }
}