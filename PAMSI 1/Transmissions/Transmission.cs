namespace PAMSI_1;

public class Transmission
{
    public Packet[] Packets { get; protected set; } = null!;
    public Guid Id { get; init; }
    public int Lenght => Packets.Length;
    public TransmissionHeader Header { get; protected set; }
    
}

public readonly record struct TransmissionHeader(Guid Id, int PacketCount);
