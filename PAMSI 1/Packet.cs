﻿namespace PAMSI_1;

public readonly record struct Packet
{
    public int Index { get; init; }
    public Guid TransmissionId { get; init; }
    public string Data { get; init; }

    public override string ToString() => $"Transmission Id: {TransmissionId}, Index: {Index}, Data: {Data}";
}