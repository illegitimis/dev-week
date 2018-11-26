namespace DevWeek.Algo
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;

    public sealed class BlockingCollectionZipProcessor : AbstractZipProcessor, IDisposable
    {
        private readonly BlockingCollection<InternalProcessZipItemModel> _bc = null;
        volatile int i;

        public BlockingCollectionZipProcessor(IReadQrCode readQrCode, IPickStockPrice pickStockPrice)
            : base (readQrCode, pickStockPrice)
        {
            _bc = new BlockingCollection<InternalProcessZipItemModel>(boundedCapacity: 4);
            i = 0;
        }

        public void Dispose()
        {
            _bc.Dispose();
        }

        protected override async Task<List<ProcessZipItemModel>> ProcessInternalAsync(MemoryStream zipStream)
        {
            var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: false);
            int numberOfPngs = zipArchive.Entries.Count;
            var bag = new ConcurrentBag<ProcessZipItemModel>();

            Action producerAction = () => 
            {
                while (!_bc.IsAddingCompleted)
                {
                    if (i >= numberOfPngs)
                    {
                        _bc.CompleteAdding();
                        break;
                    }

                    var entry = zipArchive.Entries[i];
                    try
                    {
                        var zipArchiveImageStream = entry.Open();
                        _bc.Add(new InternalProcessZipItemModel(float.NaN, float.NaN, entry.FullName, zipArchiveImageStream));
                    }
                    catch
                    {
                        _bc.Add(new InternalProcessZipItemModel(float.NaN, float.NaN, entry.FullName, Stream.Null));
                    }
                    i++;
                }
            };
            Task producerTask = new Task(producerAction);

            Action consumerAction = () => 
            {
                if (_bc.IsAddingCompleted) return;

                //_bc.GetConsumingEnumerable()
                InternalProcessZipItemModel input = null;
                while (!_bc.IsAddingCompleted || _bc.TryTake(out input))
                {
                    if (input == null) continue;
                    string output = qrCodeReader.DecodePngStream(input.zipArchiveImageStream);
                    var gain = stockPricePicker.GetMinMaxGain(output);
                    bag.Add (new ProcessZipItemModel(gain.Min, gain.Max, input.File));
                    input.Dispose();
                }

            };
            Task consumerTask = new Task(consumerAction);

            await Task.WhenAll(consumerTask, producerTask);

            // dispose archive stream & tasks
            zipArchive.Dispose();
            consumerTask.Dispose();
            producerTask.Dispose();

            return new List<ProcessZipItemModel>(bag);
        }
    }
}
