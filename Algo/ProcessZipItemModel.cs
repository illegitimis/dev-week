namespace DevWeek.Algo
{
    public class ProcessZipItemModel
    {
        private readonly float min;
        private readonly float max;
        private readonly string fullName;

        public ProcessZipItemModel(float min, float max, string fullName)
        {
            this.min = min;
            this.max = max;
            this.fullName = fullName;            
        }

        public ProcessZipItemModel(InternalProcessZipItemModel x)
            : this(x.Min, x.Max, x.File)
        {
        }

        public string File => fullName;
        public float Min => min;
        public float Max => max;
    }
}