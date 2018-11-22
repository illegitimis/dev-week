namespace DevWeek.Algo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;

    public class Unzipper : IUnzip
    {
        readonly IRunBackgroundJob backgroundJobRunner;
        
        readonly IReadQrCode qrCodeReader;

        readonly IPickStockPrice stockPricePicker;

        public Unzipper(IRunBackgroundJob jobRunner, IReadQrCode readQrCode, IPickStockPrice pickStockPrice)
        {
            backgroundJobRunner = jobRunner ?? throw new ArgumentNullException(nameof(jobRunner));
            qrCodeReader = readQrCode ?? throw new ArgumentNullException(nameof(readQrCode));
            stockPricePicker = pickStockPrice ?? throw new ArgumentNullException(nameof(pickStockPrice));
        }

        public IEnumerable<(string name, long length)> GetMetadata(string fileName)
        {
            using (ZipArchive zipArchive = ZipFile.Open(fileName, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    using (Stream s = entry.Open())
                    {
                        yield return (entry.FullName, s.Length);
                    }
                }
            }
        }

        public IEnumerable<(string name, long length)> GetMetadata(Stream zipStream)
        {
            using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: false))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    using (Stream s = entry.Open())
                    {
                        yield return (entry.FullName, s.Length);
                    }
                }
            }
        }

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

#pragma warning disable S4457 // Parameter validation in "async"/"await" methods should be wrapped
        public async Task<List<(string, float, float)>> Process(MemoryStream zipStream)
#pragma warning restore S4457 // Parameter validation in "async"/"await" methods should be wrapped
        {
            if (zipStream == null) throw new ArgumentNullException(nameof(zipStream));
            if (zipStream == Stream.Null) throw new ArgumentException(nameof(zipStream));
            if (!zipStream.CanRead) throw new InvalidOperationException(nameof(zipStream.CanRead));

            ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: false);
            var tasks = new List<Task<ProcessZipItemModel>>();
            
            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                Stream zipArchiveImageStream = entry.Open();

                Func<ProcessZipItemModel> processZipFile = () =>
                {
                    string output = qrCodeReader.DecodePngStream(zipArchiveImageStream);
                    var gain = stockPricePicker.GetMinMaxGain(output);
                    return new ProcessZipItemModel(gain.Min, gain.Max, entry.FullName, zipArchiveImageStream);
                };

                var task = Task.Factory.StartNew(processZipFile, TaskCreationOptions.LongRunning);

                tasks.Add(task);
            }

            // aggregate on all tasks completion
            var taskResults = await Task.WhenAll(tasks);

            var returns = new List<(string, float, float)>();

            foreach (var processZipItemModel in taskResults)
            {
                returns.Add ((processZipItemModel.File, processZipItemModel.Min, processZipItemModel.Max));

                // dispose each archive entry on subscribe
                processZipItemModel.Dispose();
            }                

            // dispose archive stream
            zipArchive.Dispose();

            return returns;
        }

    }
}
