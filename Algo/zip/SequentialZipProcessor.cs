namespace DevWeek.Algo
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;

    public class SequentialZipProcessor : AbstractZipProcessor
    {
        public SequentialZipProcessor(IReadQrCode readQrCode, IPickStockPrice pickStockPrice) : base(readQrCode, pickStockPrice)
        {
        }

        protected override Task<List<ProcessZipItemModel>> ProcessInternalAsync(MemoryStream zipStream)
        {
            var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: false);

            var models = new List<ProcessZipItemModel>();

            foreach (var entry in zipArchive.Entries)
            {
                // open zip stream
                var zipArchiveImageStream = entry.Open();

                // compute
                string output = qrCodeReader.DecodePngStream(zipArchiveImageStream);
                var gain = stockPricePicker.GetMinMaxGain(output);

                // store processed item output
                models.Add(new ProcessZipItemModel(gain.Min, gain.Max, entry.FullName));

                // item cleanup
                zipArchiveImageStream.Dispose();
            }

            // parent cleanup
            zipArchive.Dispose();

            return Task.FromResult(models);
        }
    }
}
