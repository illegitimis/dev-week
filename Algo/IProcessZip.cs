namespace DevWeek.Algo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public interface IProcessZip
    {
        Task<List<ProcessZipItemModel>> ProcessAsync(MemoryStream zipStream);
    }
}
