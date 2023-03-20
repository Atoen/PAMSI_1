using PAMSI_1.DataStructures;
using PAMSI_1.Logging;

namespace PAMSI_1;

public delegate void PacketTransmittedEventHandler(object sender, Packet packet);
public delegate void TransmissionStartedEventHandler(object sender, TransmissionHeader header);

public class Server
{
    public event PacketTransmittedEventHandler? PacketTransmitted;
    public event TransmissionStartedEventHandler? TransmissionStarted;

    public int PacketSize { get; set; } = 100;

    private readonly HashSet3<ushort> _activeTransmissions = new();
    private readonly ILogger _logger = new Logger("Server", LogLevel.Trace);

    public void SendMessage(string message)
    {
        var transmissionId = GenerateTransmissionId();

        var packets = CreatePackets(message, PacketSize, transmissionId);
        var header = new TransmissionHeader(transmissionId, packets.Length);

        StartTransmission(header, packets);
    }

    private Packet[] CreatePackets(string message, int packetSize, ushort id)
    {
        var packetCount = (int) Math.Ceiling((double) message.Length / packetSize);
        var packets = new Packet[packetCount];

        for (int i = 0, packetIndex = 0; i < message.Length; i += packetSize, packetIndex++)
        {
            var length = Math.Min(packetSize, message.Length - i);

            var chunk = message.Substring(i, length);

            packets[packetIndex] = new Packet(packetIndex, id, chunk);
        }

        return packets;
    }

    private void StartTransmission(TransmissionHeader header, Packet[] packets)
    {
        _activeTransmissions.Add(header.Id);
        _logger.LogInfo($"Transmission {header.Id} started.");

        TransmissionStarted?.Invoke(this, header);

        var packetTasks = packets.Select(SendPacket).ToArray();
        Task.WhenAll(packetTasks);

        _logger.LogTrace($"Transmission {header.Id} packets ({header.PacketCount}) have been sent.");
    }

    private async Task SendPacket(Packet packet)
    {
        var millisecondsDelay = Random.Shared.Next(100, 1000);
        await Task.Delay(millisecondsDelay);

        PacketTransmitted?.Invoke(this, packet);
    }

    private ushort GenerateTransmissionId()
    {
        var randomId = (ushort) Random.Shared.Next(0, ushort.MaxValue);

        while (_activeTransmissions.Contains(randomId))
        {
            randomId = (ushort) Random.Shared.Next(0, ushort.MaxValue);
        }

        return randomId;
    }

    public void CloseTransmission(object sender, ushort id)
    {
        _activeTransmissions.Remove(id);
    }
}
