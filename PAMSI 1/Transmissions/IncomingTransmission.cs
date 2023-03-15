namespace PAMSI_1;

public class IncomingTransmission : Transmission
{
    public IncomingTransmission(int expectedLength)
    {
        Packets = new Packet[expectedLength];
        RemainingPackets = expectedLength;
    }
    
    public int RemainingPackets { get; set; }
    public string Data => string.Join("", Packets.Select(p => p.Data));

    public void AddPacket(Packet packet)
    {
        Packets[packet.Index] = packet;
    }
}
