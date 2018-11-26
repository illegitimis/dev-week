namespace DevWeek.Algo
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;

    public sealed class ParallelCpuBoundZipProcessor : AbstractZipProcessor
    {
        private readonly ConcurrentBag<InternalProcessZipItemModel> q = null;

        public ParallelCpuBoundZipProcessor(IReadQrCode readQrCode, IPickStockPrice pickStockPrice)
            : base(readQrCode, pickStockPrice)
        {
            q = new ConcurrentBag<InternalProcessZipItemModel>();
        }

        

        protected override async Task<List<ProcessZipItemModel>> ProcessInternalAsync(MemoryStream zipStream)
        {
            var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: false);
            
            var x = new ConcurrentQueue<ProcessZipItemModel>();

            Task t1 = Task.Factory.StartNew(() => 
            {
                foreach (var entry in zipArchive.Entries)
                {
                    // open zip stream
                    var zipArchiveImageStream = entry.Open();

                    // PushItemToProcessingQueue
                    q.Add(new InternalProcessZipItemModel(float.NaN, float.NaN, entry.FullName, zipArchiveImageStream));
                }
            });


            Task t2 = Task.Factory.StartNew(() =>
            {
                // CPU-BOUND-zone
                Action action = () =>
                {
                    while (q.TryTake(out InternalProcessZipItemModel ipzim))
                    {
                        // compute
                        string output = qrCodeReader.DecodePngStream(ipzim.zipArchiveImageStream);
                        var gain = stockPricePicker.GetMinMaxGain(output);

                        // store
                        x.Enqueue(new ProcessZipItemModel(gain.Min, gain.Max, ipzim.File));

                        // cleanup entry
                        ipzim.zipArchiveImageStream.Dispose();
                    }
                };

                Parallel.Invoke(action, action, action);
            });

            await Task.WhenAll(t1, t2);

            // parent cleanup
            zipArchive.Dispose();
            t1.Dispose();
            t2.Dispose();

            return new List<ProcessZipItemModel> (x);
        }
    }
}
