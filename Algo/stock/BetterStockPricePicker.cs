namespace DevWeek.Algo
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Amortized T(1.5*N)
    /// </summary>
    /// <remarks>
    /// worst case: array sorted descending => 1 + 2(n-2) = 2n-1 comparisons
    /// best case: array sorted ascending => 1 + (n-2) = n-1 comparisons
    /// average: 1 + p*(n-2), p between [1,2] ~ T(1.5n)
    /// </remarks>
    public class BetterStockPricePicker : AbstractStockPricePicker
    {
        public override (float Min, float Max) Get(float[] ticks)
        {
            if (ticks == null) throw new ArgumentNullException(nameof(ticks));
            if (ticks.Length == 0) throw new ArgumentException("zero length", nameof(ticks));
            // only one element => impossible
            // if (ticks.Length == 1) return (ticks[0], float.NaN); 
            // two elements => either way order (min, max) or (max, min) invalid
            // if (ticks.Length == 2) return (Math.Min(ticks[0], ticks[1]), float.NaN);

            float min = ticks[0], max = ticks[0];
            int minidx = 0;

            for (int i = 1; i < ticks.Length; i++)
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
            }

            return (min, max);
        }
    }

    public class AggressiveInliningBetterStockPricePicker : BetterStockPricePicker
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override (float Min, float Max) Get(float[] ticks) => base.Get(ticks);
    }

    public class NoInliningBetterStockPricePicker : BetterStockPricePicker
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override (float Min, float Max) Get(float[] ticks) => base.Get(ticks);
    }

    public class NoOptimizationBetterStockPricePicker : BetterStockPricePicker
    {
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public override (float Min, float Max) Get(float[] ticks) => base.Get(ticks);
    }
}