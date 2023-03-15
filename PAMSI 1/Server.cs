namespace PAMSI_1;

public delegate void PacketReceivedEventHandler(object sender, Packet packet);

public delegate void TransmissionStartedEventHandler(object sender, TransmissionHeader header);

public class Server
{
    public event PacketReceivedEventHandler? PacketReceived;
    public event TransmissionStartedEventHandler? TransmissionStarted;

    public void SendMessage(OutgoingTransmission outgoingTransmission)
    {
        outgoingTransmission.Packets.Shuffle();
        
        TransmissionStarted?.Invoke(this, outgoingTransmission.Header);

        foreach (var packet in outgoingTransmission.Packets)
        {
            PacketReceived?.Invoke(this, packet);
        }
    }
}
