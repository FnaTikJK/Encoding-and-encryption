// See https://aka.ms/new-console-template for more information

using System.Drawing.Imaging;
using System.Text;
using Qr_Coder;

var text = "фыв"; // 5 version "Это прсосто какой-то рандомный текстапывпваывааыва"
// text = string.Join("", Enumerable.Repeat(text, 3)); // 11 version
var data = Encoding.Default.GetBytes(text);
var correction = Correction.L;
var image = Coder.Encode(data, correction);
image.Save(@"C:\Users\Антон\source\repos\c#\Encoding-and-encryption\Qr-Coder\image.png", ImageFormat.Png);