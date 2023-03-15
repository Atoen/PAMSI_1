using System.Text;

namespace PAMSI_1;

public class Receiver
{
    public Receiver(Server server)
    {
        _server = server;
        
        _server.TransmissionStarted += ServerOnTransmissionStarted;
        _server.PacketReceived += ServerOnPacketReceived;
    }
    
    private readonly Server _server;
    private readonly Dictionary<Guid, IncomingTransmission> _activeTransmissions = new();

    private void ServerOnTransmissionStarted(object sender, TransmissionHeader header)
    {
        Console.WriteLine($"Transmission {header.Id} started, expecting {header.PacketCount} packets.");

        _activeTransmissions.Add(header.Id, new IncomingTransmission(header.PacketCount)
        {
            Id = header.Id
        });
    }
    
    private void ServerOnPacketReceived(object sender, Packet packet)
    {
        if (!_activeTransmissions.TryGetValue(packet.TransmissionId, out var incomingTransmission))
        {
            throw new InvalidDataException(
                $"Received packet's id did not match any of open transmissions'. Packet: {packet}");
        }

        Console.WriteLine($"Received packet {{{packet}}}");

        incomingTransmission.AddPacket(packet);
        incomingTransmission.RemainingPackets--;
        
        if (incomingTransmission.RemainingPackets == 0)
        {
            CloseTransmission(incomingTransmission);
        }
    }

    private void CloseTransmission(IncomingTransmission transmission)
    {
        Console.WriteLine($"Transmission {transmission.Id} has finished.");
        Console.WriteLine($"Data received: {transmission.Data}");

        _activeTransmissions.Remove(transmission.Id);
    }
}