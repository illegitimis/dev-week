namespace DevWeek.Algo
{
    using System;
    using System.IO;

    internal class ProcessZipItemModel : IDisposable
    {
        private readonly float min;
        private readonly float max;
        private readonly string fullName;
        private readonly Stream zipArchiveImageStream;

        public ProcessZipItemModel(float min, float max, string fullName, Stream zipArchiveImageStream)
        {
            this.min = min;
            this.max = max;
            this.fullName = fullName;
            this.zipArchiveImageStream = zipArchiveImageStream;
        }

        public string File => fullName;
        public float Min => min;
        public float Max => max;

        public void Dispose()
        {
            zipArchiveImageStream.Dispose();
        }
    }
}