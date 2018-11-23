namespace DevWeek.Algo
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;

    public class ParallelForEachZipProcessor : AbstractZipProcessor
    {
        public ParallelForEachZipProcessor(IReadQrCode readQrCode, IPickStockPrice pickStockPrice)
            : base(readQrCode, pickStockPrice)
        {
        }

        protected override Task<List<ProcessZipItemModel>> ProcessInternalAsync(MemoryStream zipStream)
        {
            var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: false);

            var models = new ConcurrentBag<ProcessZipItemModel>();

            Parallel.ForEach(zipArchive.Entries, new ParallelOptions { MaxDegreeOfParallelism = 4 }, entry =>
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
            });

            // parent cleanup
            zipArchive.Dispose();

            return Task.FromResult (new List<ProcessZipItemModel> (models));
        }
    }
}
