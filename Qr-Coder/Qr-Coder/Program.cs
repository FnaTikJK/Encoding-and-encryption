// See https://aka.ms/new-console-template for more information

using System.Text;
using Qr_Coder;

var data = Encoding.Default.GetBytes("Это прсосто какой-то рандомный текстапывпваывааыва");
var correction = Correction.M;
Coder.Encode(data, correction);