namespace DevWeek.Algo
{
    using System;
    using System.Threading.Tasks;

    public class ParallelForStockPricePicker : AbstractStockPricePicker
    {
        /// <summary>
        /// inserts a memory barrier that prevents the processor from reordering memory operations as follows: 
        /// If a read or write appears after this method in the code, the processor cannot move it before this method.
        /// Guarantess reads / writes to shared variables is stable / ordered.
        /// </summary>
        volatile float min = float.MaxValue;
        volatile float max = float.MinValue;
        volatile int minidx = 0;

        public override (float Min, float Max) Get(float[] ticks)
        {
            if (ticks == null) throw new ArgumentNullException(nameof(ticks));
            if (ticks.Length == 0) throw new ArgumentException("zero length", nameof(ticks));
            // only one element => impossible
            // if (ticks.Length == 1) return (ticks[0], float.NaN); 
            // two elements => either way order (min, max) or (max, min) invalid
            // if (ticks.Length == 2) return (Math.Min(ticks[0], ticks[1]), float.NaN);

            Parallel.For(1, ticks.Length, (i, state) => 
            {
                if (min > ticks[i])
                {
                    // reset maximum and memoize minimum index in array
                    min = ticks[i];
                    max = ticks[i];
                    minidx = i;
                }
                else if (max < ticks[i] && i - minidx > 1)
                {
                    max = ticks[i];
                }
            });

            return (min, max);
        }
    }
}
