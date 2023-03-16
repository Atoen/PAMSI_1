namespace PAMSI_1;

public record Packet
{
    public int Index { get; init; }
    // public Guid TransmissionId { get; init; }

    public ushort TransmissionId { get; init; }
    public required string Data { get; init; }

    public override string ToString() => $"{{Transmission Id: {TransmissionId}, Index: {Index}, Data: {Data}}}";
}
