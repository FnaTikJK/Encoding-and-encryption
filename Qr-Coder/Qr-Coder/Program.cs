// See https://aka.ms/new-console-template for more information

using System.Drawing.Imaging;
using System.Text;
using Qr_Coder;

var text = "NIGGERS";
var correction = Correction.M;
var image = Qr_Coder.QrCoder.Encode(text, correction);
image.Save(@"C:\Users\Антон\source\repos\c#\Encoding-and-encryption\Qr-Coder\image.png", ImageFormat.Png);