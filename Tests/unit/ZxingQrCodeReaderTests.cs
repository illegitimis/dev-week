namespace Tests.Unit
{
    using DevWeek.Algo;
    using System.Diagnostics;
    using System.IO;
    using Xunit;
    using static Paths;
    using static Strings;

    public class ZxingQrCodeReaderTests
    {
        private readonly ZxingQrCodeReader zxingQrCodeReader;

        public ZxingQrCodeReaderTests()
        {
            zxingQrCodeReader = new ZxingQrCodeReader();
        }

        [Theory]
        [InlineData(TenTicksPng, TenTicks)]
        [InlineData(HundredTicksPng, OneHundredTicks)]
        [InlineData("qrcode.online.png", OneHundredTicks + " " + TenTicks)]
        public void DecodesQRCodeToString(string fileName, string expectedString)
        {
            var stringResult = zxingQrCodeReader.DecodePngFile($"{PicsFolder}{fileName}");
            Assert.Equal(expectedString, stringResult);
        }

        [Fact]
        public void Performance()
        {
            using (FileStream fs = File.OpenWrite("qr.csv"))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.BaseStream.Seek(0, SeekOrigin.End);

                foreach (string pngPath in Directory.GetFiles(PicsFolder, "*.png"))
                {

                    var stopwatch = Stopwatch.StartNew();
                    var qrEncodedString = zxingQrCodeReader.DecodePngFile(pngPath);
                    stopwatch.Stop();

                    Assert.NotEmpty(qrEncodedString);
                    sw.WriteLine($"{pngPath}, {qrEncodedString}, {stopwatch.ElapsedTicks}");
                }
            }
        }
    }
}
