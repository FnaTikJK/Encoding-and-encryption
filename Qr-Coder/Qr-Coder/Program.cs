using System.Drawing.Imaging;
using Qr_Coder;


while(true)
try
{
    Console.WriteLine("Input command like: L/M/Q/H {text}");
    var command = Console.ReadLine()!.Split();
    var correction = Enum.Parse<Correction>(command[0].ToUpper());
    var text = string.Join(" ", command[1..]);
    var image = QrCoder.Encode(text, correction, 1);
    image.Save($"qr-code.png", ImageFormat.Png);
    Console.WriteLine("Success!");
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}