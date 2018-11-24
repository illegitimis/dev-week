namespace DevWeek.Algo
{
    using System;
    using System.Linq;

    /// <summary>
    /// Abstract base class for picking best gain stock prices algorithms
    /// </summary>
    public abstract class AbstractStockPricePicker : IPickStockPrice
    {
        public abstract (float Min, float Max) Get(float[] ticks);

        public virtual (float Min, float Max) GetMinMaxGain(string dailyStockPriceTicks)
        {
            try
            {
                var ticks = dailyStockPriceTicks
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(token => float.Parse(token))
                        .ToArray();

                return Get(ticks);
            }
            catch
            {
                return (float.NaN, float.NaN);
            }
        }
    }
}
