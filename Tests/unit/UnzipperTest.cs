namespace Tests.Unit
{
    using DevWeek.Algo;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    using static Paths;

    public sealed class UnzipperTest
    {
        static readonly IReadQrCode qrCodeReader = new ZxingQrCodeReader();

        static readonly IPickStockPrice stockPricePicker = new CheatingStockPricePicker();

        [Theory]
        [MemberData(nameof(ZipProcessors))]
        public async Task ProcessesMinimalInput(AbstractZipProcessor zipProcessor)
        {
            var bytes = await File.ReadAllBytesAsync($"{ZipsFolder}{InputsZip}");
            using (var ms = new MemoryStream(bytes))
            {
                var results = await zipProcessor.ProcessAsync(ms);
                Assert.Equal(2, results.Count);
                Assert.Equal(TenTicksPng, results[0].File);
                // Assert.Equal(403, results[0].length);
                Assert.Equal(HundredTicksPng, results[1].File);
                // Assert.Equal(1055, results[1].length);
            }   
        }

        public static IEnumerable<object[]> ZipProcessors()
        {
            yield return new object[] { new SequentialZipProcessor(qrCodeReader, stockPricePicker) };
            yield return new object[] { new HangfireLikeZipProcessor(qrCodeReader, stockPricePicker) };
        }
    }
}
