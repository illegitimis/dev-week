namespace DevWeek.Algo
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// number of comparisons can be reduced by comparing pairs first.
    /// </summary>
    public class PairsStockPricePicker : AbstractStockPricePicker
    {
        public override (float Min, float Max) Get(float[] ticks)
        {
            if (ticks == null) throw new ArgumentNullException(nameof(ticks));
            if (ticks.Length == 0) throw new ArgumentException("zero length", nameof(ticks));
            if (ticks.Length < 3 ) return (ticks[0], float.NaN);

            float min, max;
            int minidx = 0;

            if (ticks[0] < ticks[1])
            {
                min = ticks[0];
                max = ticks[0];
                minidx = 0;
            }
            else
            {
                min = ticks[1];
                max = ticks[1];
                minidx = 1;
            }

            for (int i = 2; i < ticks.Length - 1; i += 2)
            {
                if (ticks[i] > ticks[i + 1])
                {
                    if (ticks[i+1] < min)
                    {
                        min = ticks[i + 1];
                        max = ticks[i + 1];
                        minidx = i+1;
                    }
                    else if (max < ticks[i] && i - minidx > 1)
                    {
                        max = ticks[i];
                    }
                }
                else //ticks[i] <= ticks[i + 1]
                {
                    if (ticks[i] < min)
                    {
                        min = ticks[i];
                        max = ticks[i];
                        minidx = i;
                    }
                    else if (max < ticks[i+1] && i > minidx)
                    {
                        max = ticks[i + 1];
                    }
                }
            }

            // array size is odd, n is last index
            if (ticks.Length % 2 == 1)
            {
                int n = ticks.Length - 1;
                if (ticks[n] < min)
                {
                    return (ticks[n], ticks[n]);
                }
                else if (max < ticks[n])
                {
                    max = ticks[n];
                }
            }

            return (min, max);
        }
    }
}
