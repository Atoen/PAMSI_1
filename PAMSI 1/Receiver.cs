using PAMSI_1.DataStructures;
using PAMSI_1.Logging;

namespace PAMSI_1;

public delegate void TransmissionFinishedEventHandler(object sender, Transmission transmission);

public class Receiver
{
    public Receiver(Server server)
    {
        _server = server;

        _server.TransmissionStarted += ServerOnTransmissionStarted;
        _server.PacketTransmitted += ReceivePacket;

        TransmissionFinished += (sender, transmission) => _server.CloseTransmission(sender, transmission.Id);
    }

    public event TransmissionFinishedEventHandler? TransmissionFinished;

    private readonly SimpleArrayList<Transmission> _incomingTransmissions = new();

    private readonly object _syncRoot = new();
    private readonly Server _server;

    private readonly ILogger _logger = new Logger("Receiver", LogLevel.Trace);

    private void ServerOnTransmissionStarted(object sender, TransmissionHeader header)
    {
        _logger.LogInfo($"Incoming transmission {header.Id}, expecting {header.PacketCount} packets.");

        _incomingTransmissions.Add(new Transmission(header));
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
        var transmission = _incomingTransmissions.Find(t => t.Id == packet.TransmissionId);

        if (transmission == null)
        {
            _logger.LogWarning($"Received packet is not bound to any open transmission. Packet: {packet}.");
            return;
        }

        _logger.LogTrace($"Received {packet}.");
        transmission.ReceivePacket(packet);

        if (transmission.Completed)
        {
            FinishTransmission(transmission);
        }
    }

    private void FinishTransmission(Transmission transmission)
    {
        _incomingTransmissions.Remove(transmission);

        _logger.LogInfo($"Transmission {transmission.Id} data received.");
        TransmissionFinished?.Invoke(this, transmission);
    }
}
