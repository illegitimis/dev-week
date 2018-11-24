namespace DevWeek.Algo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;

    public sealed class HangfireLikeZipProcessor : AbstractZipProcessor
    {

        public HangfireLikeZipProcessor(IReadQrCode readQrCode, IPickStockPrice pickStockPrice)
            : base(readQrCode, pickStockPrice)
        {
        }

        protected override async Task<List<ProcessZipItemModel>> ProcessInternalAsync(MemoryStream zipStream)
        {
            var tasks = new List<Task<InternalProcessZipItemModel>>();

            ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: false);

            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                Stream zipArchiveImageStream = Stream.Null;

                try
                {
                    zipArchiveImageStream = entry.Open();
                }
                catch
                {
                    // ignore unzip errors
                }

                if (zipArchiveImageStream == Stream.Null) continue;

                Func<InternalProcessZipItemModel> processZipFile = () =>
                {
                    string output = qrCodeReader.DecodePngStream(zipArchiveImageStream);
                    var gain = stockPricePicker.GetMinMaxGain(output);
                    return new InternalProcessZipItemModel(gain.Min, gain.Max, entry.FullName, zipArchiveImageStream);
                };

                var task = Task.Factory.StartNew(processZipFile, TaskCreationOptions.LongRunning);

                tasks.Add(task);
            }

            // aggregate on all tasks completion
            var taskResults = await Task.WhenAll(tasks);

            var returns = new List<ProcessZipItemModel>();

            foreach (var taskResult in taskResults)
            {
                returns.Add(new ProcessZipItemModel(taskResult));

                // dispose each archive entry on subscribe
                taskResult.Dispose();
            }

            // dispose archive stream
            zipArchive.Dispose();

            return returns;
        }
    }
}
