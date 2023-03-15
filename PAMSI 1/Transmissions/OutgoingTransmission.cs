namespace PAMSI_1;

public class OutgoingTransmission : Transmission
{
    public OutgoingTransmission(string message, int packetLenght)
    {
        Id = Guid.NewGuid();
        PacketLength = packetLenght;
        
        SplitMessage(message);
        
        Header = new TransmissionHeader(Id, Lenght);
    }

    public int PacketLength { get; }

    private void SplitMessage(string message)
    {
        var packetCount = (int) Math.Ceiling((double) message.Length / PacketLength);
        Packets = new Packet[packetCount];
        
        for (int i = 0, p = 0; i < message.Length; i += PacketLength, p++)
        {
            var length = Math.Min(PacketLength, message.Length - i);

            var chunk = message.Substring(i, length);

            Packets[p] = new Packet
            {
                Index = p,
                TransmissionId = Id,
                Data = chunk
            };
        }
    }
}

