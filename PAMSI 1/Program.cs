using PAMSI_1;

Console.WriteLine("Enter message to send:");

var message = Console.ReadLine();

// message = "example implementation";

if (string.IsNullOrWhiteSpace(message))
{
    Console.WriteLine("Message should not be empty.");
    return;
}

Console.Write("Select packet lenght: ");
var packetLenghtInput = Console.ReadLine();

// packetLenghtInput = "5";

if (!int.TryParse(packetLenghtInput, out var packetLenght) || packetLenght < 1)
{
    Console.WriteLine("Packet lenght must be a whole number not less than 1.");
    return;
}

var server = new Server();

var sender = new Sender(server)
{
    PacketLength = packetLenght
};

var receiver = new Receiver(server);

sender.SendMessage(message);

Console.Read();
