namespace DevWeek.Algo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public interface IUnzip
    {
        IEnumerable<(string name, long length)> GetMetadata(string fileName);
        IEnumerable<(string name, long length)> GetMetadata(Stream zipStream);
        List<Task<(string, object)>> ProcessZipImages(Stream zipStream, Func<Stream, object> imageStreamProcessor);
        Task<List<(string, float, float)>> Process(MemoryStream zipStream);
    }
}
