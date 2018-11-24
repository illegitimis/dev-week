using System.IO;

namespace DevWeek.Algo
{
    public interface IReadQrCode
    {
        string DecodePngFile(string fileName);
        string DecodePngStream(Stream pngStream);
    }
}