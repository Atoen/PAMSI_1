using PAMSI_1.Logging;

namespace PAMSI_1;

public delegate void TransmissionFinishedEventHandler(object sender, ushort transmissionId);

public class Receiver
{
    public Receiver(Server server)
    {
        _server = server;

        _server.TransmissionStarted += ServerOnTransmissionStarted;
        _server.PacketSent += ReceivePacket;

        TransmissionFinished += _server.CloseTransmission;
    }

    public event TransmissionFinishedEventHandler? TransmissionFinished;

    private readonly Dictionary<ushort, Transmission> _incomingTransmission = new();

    private readonly object _syncRoot = new();
    private readonly Server _server;

    private readonly ILogger _logger = new Logger("Receiver", LogLevel.Trace);

    private void ServerOnTransmissionStarted(object sender, TransmissionHeader header)
    {
        _logger.LogInfo($"Incoming transmission {header.Id}, expecting {header.PacketCount} packets.");

        _incomingTransmission.Add(header.Id, new Transmission(header));
    }

    private void ReceivePacket(object sender, Packet packet)
    {
        lock (_syncRoot)
        {
            ProcessPacket(packet);
        }
    }

    private void ProcessPacket(Packet packet)
    {
        if (!_incomingTransmission.TryGetValue(packet.TransmissionId, out var transmission))
        {
            _logger.LogWarning($"Received packet is not bound to any open transmission. Packet: {packet}");
            return;
        }

        _logger.LogTrace($"Received packet {packet}");

        transmission.ReceivePacket(packet);

        if (!transmission.Completed) return;

        TransmissionFinished?.Invoke(this, transmission.Id);
        FinishTransmission(transmission);
    }

    private void FinishTransmission(Transmission transmission)
    {
        _incomingTransmission.Remove(transmission.Id);

        _logger.LogInfo($"Transmission {transmission.Id} data has been received: {transmission.Data}");
    }
}
