namespace DevWeek.Algo
{
    using System;
    using System.IO;

    public sealed class InternalProcessZipItemModel : ProcessZipItemModel, IDisposable
    {
        internal readonly Stream zipArchiveImageStream;
        
        public InternalProcessZipItemModel(float min, float max, string fullName, Stream zipArchiveImageStream)
            : base (min, max, fullName)
        {
            this.zipArchiveImageStream = zipArchiveImageStream;
        }
        public void Dispose()
        {
            zipArchiveImageStream.Dispose();
        }
    }
}