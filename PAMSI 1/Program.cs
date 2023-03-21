using PAMSI_1;

Console.WriteLine("Enter message to send:");
var messageInput = Console.ReadLine();

if (string.IsNullOrWhiteSpace(messageInput))
{
    Console.WriteLine("Message should not be empty.");
    return;
}

Console.Write("Select packet length: ");
var packetLengthInput = Console.ReadLine();

if (!int.TryParse(packetLengthInput, out var packetLength) || packetLength < 1)
{
    Console.WriteLine("Packet length must be a whole number not less than 1.");
    return;
}

var server = new Server
{
    PacketSize = packetLength
};

var sender = new Sender(server);

var receiver = new Receiver(server);
receiver.TransmissionFinished += delegate(object _, Transmission transmission)
{
    Console.WriteLine(transmission.Data);
};

sender.SendMessage(messageInput);

Console.Read();
