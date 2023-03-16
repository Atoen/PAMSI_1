namespace PAMSI_1;

public delegate void TransmissionFinishedEventHandler(object sender, ushort transmissionId);

public class Receiver
{
    public Receiver(Server server)
    {
        _server = server;

        _server.TransmissionStarted += ServerOnTransmissionStarted;
        _server.PacketReceived += ServerOnPacketReceived;

        TransmissionFinished += _server.CloseTransmission;
    }

    public event TransmissionFinishedEventHandler? TransmissionFinished;

    private readonly object _syncRoot = new();
    private readonly Server _server;

    private void ServerOnTransmissionStarted(object sender, TransmissionHeader header)
    {
        Console.WriteLine($"Incoming transmission {header.Id}, expecting {header.PacketCount} packets.");
    }

    private void ServerOnPacketReceived(object sender, Packet packet)
    {
        lock (_syncRoot)
        {
            ProcessPacket(packet);
        }
    }

    private void ProcessPacket(Packet packet)
    {
        if (!_server.ActiveTransmissions.TryGetValue(packet.TransmissionId, out var transmission))
        {
            Console.WriteLine($"Received packet is not bound to any open transmission. Packet: {packet}");
            return;
        }

        Console.WriteLine($"Received packet {packet}");

        transmission.ReceivePacket(packet);

        if (!transmission.Completed) return;

        TransmissionFinished?.Invoke(this, transmission.Id);
        ReadData(transmission);
    }

    private void ReadData(Transmission transmission)
    {
        Console.WriteLine($"Transmission {transmission.Id} data has been received: {transmission.Data}");
    }
}
