using PAMSI_1;
using PAMSI_1.DataStructures;

var hashSet = new HashSet3<int>
{
    1,
    1,
    2,
    2,
    3,
    4,
    4,
    4,
    4,
    4,
    12,
    45,
    77,
    4,
    4
};

// hashSet.Remove(3);

foreach (var element in hashSet)
{
    Console.WriteLine(element);
}

Console.WriteLine(hashSet.Contains(1));

;

// Console.WriteLine("Enter message to send:");
// var message = Console.ReadLine();
//
// if (string.IsNullOrWhiteSpace(message))
// {
//     Console.WriteLine("Message should not be empty.");
//     return;
// }
//
// Console.Write("Select packet length: ");
// var packetLengthInput = Console.ReadLine();
//
// if (!int.TryParse(packetLengthInput, out var packetLength) || packetLength < 1)
// {
//     Console.WriteLine("Packet length must be a whole number not less than 1.");
//     return;
// }
//
// var server = new Server
// {
//     PacketSize = packetLength
// };
//
// var sender = new Sender(server);
//
// var receiver = new Receiver(server);
//
// sender.SendMessage(message);
//
// Console.Read();
