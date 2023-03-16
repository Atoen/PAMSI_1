namespace PAMSI_1;

public class Transmission
{
    public Transmission(TransmissionHeader header)
    {
        Id = header.Id;
        Packets = new Packet[header.PacketCount];
        PacketsLeftToDeliver = header.PacketCount;
    }

    public Packet[] Packets { get; }
    public ushort Id { get; }

    public int Lenght => Packets.Length;
    public string Data
    {
        get
        {
            if (!Completed)
            {
                throw new InvalidOperationException("Attempting to read data from an unfinished transmission");
            }

            return string.Join("", Packets.Select(p => p.Data));
        }
    }

    public int PacketsLeftToDeliver { get; private set; }

    public bool Completed => PacketsLeftToDeliver == 0;

    public void ReceivePacket(Packet packet)
    {
        Packets[packet.Index] = packet;
        PacketsLeftToDeliver--;
    }
}

public readonly record struct TransmissionHeader(ushort Id, int PacketCount);
