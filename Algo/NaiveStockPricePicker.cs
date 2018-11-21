namespace DevWeek.Algo
{
    using System;
    using System.Runtime.CompilerServices;

    public class NaiveStockPricePicker : AbstractStockPricePicker
    {
        /// <summary>
        /// Simply iterate through the array and compare twice to each element to get min and max. 
        /// This leads to 2*(n-1) comparisons.
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public override (float Min, float Max) Get(float[] ticks)
        {
            if (ticks == null) throw new ArgumentNullException(nameof(ticks));
            if (ticks.Length == 0) throw new ArgumentException("zero length", nameof(ticks));

            float min = ticks[0], max = ticks[0];
            int minidx = 0;

            for (int i = 1; i < ticks.Length; i++)
            {
                if (min > ticks[i])
                {
                    min = ticks[i];

                    // reset old maximum too, should be stable
                    max = ticks[i];

                    // remember min position
                    minidx = i;                    
                }

                if (max < ticks[i] && i - minidx > 1)
                {
                    max = ticks[i];                    
                }
            }

            return (min, max);
        }
    }

    public class AggressiveInliningNaiveStockPricePicker : NaiveStockPricePicker
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override (float Min, float Max) Get(float[] ticks) => base.Get(ticks);        
    }

    public class NoInliningNaiveStockPricePicker : NaiveStockPricePicker
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override (float Min, float Max) Get(float[] ticks) => base.Get(ticks);
    }

    public class NoOptimizationNaiveStockPricePicker : NaiveStockPricePicker
    {
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public override (float Min, float Max) Get(float[] ticks) => base.Get(ticks);
    }
}
