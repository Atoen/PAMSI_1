using PAMSI_1.Transmissions;

namespace PAMSI_1;

public delegate void PacketReceivedEventHandler(object sender, Packet packet);
public delegate void TransmissionStartedEventHandler(object sender, TransmissionHeader header);

public class Server
{
    public event PacketReceivedEventHandler? PacketReceived;
    public event TransmissionStartedEventHandler? TransmissionStarted;

    public void SendMessage(OutgoingTransmission transmission)
    {
        Console.WriteLine($"Transmission {transmission.Id} started.");

        TransmissionStarted?.Invoke(this, transmission.Header);

        var tasks = transmission.Packets.Select(SendPacket).ToArray();

        Task.WhenAll(tasks);
    }

    private async Task SendPacket(Packet packet)
    {
        var delay = Random.Shared.Next(100, 1000);
        await Task.Delay(delay);
        
        PacketReceived?.Invoke(this, packet);
    }
}
