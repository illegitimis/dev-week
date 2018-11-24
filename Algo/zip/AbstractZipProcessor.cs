namespace DevWeek.Algo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public abstract class AbstractZipProcessor : IProcessZip
    {
        // readonly IRunBackgroundJob backgroundJobRunner;
        
        protected readonly IReadQrCode qrCodeReader;

        protected readonly IPickStockPrice stockPricePicker;

        public AbstractZipProcessor(/*IRunBackgroundJob jobRunner, */ IReadQrCode readQrCode, IPickStockPrice pickStockPrice)
        {
            // backgroundJobRunner = jobRunner ?? throw new ArgumentNullException(nameof(jobRunner));
            qrCodeReader = readQrCode ?? throw new ArgumentNullException(nameof(readQrCode));
            stockPricePicker = pickStockPrice ?? throw new ArgumentNullException(nameof(pickStockPrice));
        }

      
        /*
        public List<Task<(string, object)>> ProcessZipImages(Stream zipStream, Func<Stream, object> imageStreamProcessor)
        {
            if (zipStream == null) throw new ArgumentNullException(nameof(zipStream));
            if (zipStream == Stream.Null) throw new ArgumentException(nameof(zipStream));
            if (!zipStream.CanRead) throw new InvalidOperationException(nameof(zipStream.CanRead));

            var tasks = new List<Task<(string,object)>>();

            using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: false))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    Stream zipArchiveImageStream = entry.Open();
                    // using (zipArchiveImageStream)
                    {
                        var task = backgroundJobRunner.Enqueue(() =>
                        {
                            object o = imageStreamProcessor(zipArchiveImageStream);
                            return (entry.FullName, o);
                        });
                        tasks.Add(task);                        
                    }
                }
            }
            return tasks;
        }
        */

        public Task<List<ProcessZipItemModel>> ProcessAsync(MemoryStream zipStream)

        {
            if (zipStream == null) throw new ArgumentNullException(nameof(zipStream));
            if (zipStream == Stream.Null) throw new ArgumentException(nameof(zipStream));
            if (!zipStream.CanRead) throw new InvalidOperationException(nameof(zipStream.CanRead));

            return ProcessInternalAsync(zipStream);
        }

        protected abstract Task<List<ProcessZipItemModel>> ProcessInternalAsync(MemoryStream zipStream);
    }
}
