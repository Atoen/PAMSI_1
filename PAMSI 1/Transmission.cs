using System.Text;
using PAMSI_1.DataStructures;

namespace PAMSI_1;

public class Transmission
{
    public Transmission(TransmissionHeader header)
    {
        Id = header.Id;
        PacketsReceived = new PriorityQueue2<Packet, int>();
        PacketsLeftToDeliver = header.PacketCount;
    }

    public readonly PriorityQueue2<Packet, int> PacketsReceived;

    public ushort Id { get; }

    private string _data = string.Empty;
    public string Data
    {
        get
        {
            if (!Completed)
            {
                throw new InvalidOperationException("Attempting to read data from an uncompleted transmission.");
            }

            if (_data == string.Empty) _data = GetPacketsData();
            
            return _data;
        }
    }

    private string GetPacketsData()
    {
        var builder = new StringBuilder();

        while (!PacketsReceived.IsEmpty)
        {
            var packet = PacketsReceived.Dequeue();
            builder.Append(packet.Data);
        }

        return builder.ToString();
    }

    public int PacketsLeftToDeliver { get; private set; }

    public bool Completed => PacketsLeftToDeliver == 0;

    public void ReceivePacket(Packet packet)
    {
        PacketsReceived.Enqueue(packet, packet.Index);
        PacketsLeftToDeliver--;
    }
}

public readonly record struct TransmissionHeader(ushort Id, int PacketCount);
