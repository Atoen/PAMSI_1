using System.Diagnostics;

namespace PAMSI_1;

public delegate void PacketReceivedEventHandler(object sender, Packet packet);
public delegate void TransmissionStartedEventHandler(object sender, TransmissionHeader header);

public class Server
{
    public event PacketReceivedEventHandler? PacketReceived;
    public event TransmissionStartedEventHandler? TransmissionStarted;

    public void SendMessage(string message, int packetSize)
    {
        var transmissionId = GenerateTransmissionId();

        var packets = CreatePackets(message, packetSize, transmissionId);
        var header = new TransmissionHeader(transmissionId, packets.Length);

        StartTransmission(header, packets);
    }

    private Packet[] CreatePackets(string message, int packetSize, ushort id)
    {
        var packetCount = (int) Math.Ceiling((double) message.Length / packetSize);
        var packets = new Packet[packetCount];

        for (int i = 0, p = 0; i < message.Length; i += packetSize, p++)
        {
            var length = Math.Min(packetSize, message.Length - i);

            var chunk = message.Substring(i, length);

            packets[p] = new Packet
            {
                Index = p,
                TransmissionId = id,
                Data = chunk
            };
        }

        return packets;
    }

    private void StartTransmission(TransmissionHeader header, Packet[] packets)
    {
        ActiveTransmissions.Add(header.Id, new Transmission(header));

        Console.WriteLine($"Transmission {header.Id} started.");

        TransmissionStarted?.Invoke(this, header);

        var packetTasks = packets.Select(SendPacket).ToArray();

        Task.WhenAll(packetTasks);

        Console.WriteLine($"Transmission {header.Id} packets have been sent.");
    }

    private async Task SendPacket(Packet packet)
    {
        var delay = Random.Shared.Next(100, 1000);
        await Task.Delay(delay);

        PacketReceived?.Invoke(this, packet);
    }

    public readonly Dictionary<ushort, Transmission> ActiveTransmissions = new();

    private ushort GenerateTransmissionId()
    {
        var randomId = (ushort) Random.Shared.Next(0, ushort.MaxValue);

        while (ActiveTransmissions.ContainsKey(randomId))
        {
            randomId = (ushort) Random.Shared.Next(0, ushort.MaxValue);
        }

        return randomId;
    }

    public void CloseTransmission(object sender, ushort id)
    {
        ActiveTransmissions.Remove(id);
    }
}
