namespace DevWeek.Algo
{
    public interface IPickStockPrice
    {
        /// <summary>
        /// Given a list of stock price ticks for the day, get the trades I should make to maximize my gain within the constraints of the market.
        /// </summary>
        /// <param name="ticks">market price ticks</param>
        /// <returns>Remember—buy low, sell high.</returns>
        /// <remarks>
        /// 1. You can't sell before you buy.
        /// 2. I have to wait for at least one tick to go buy.
        /// // https://www.programcreek.com/2014/02/find-min-max-in-an-array-using-minimum-comparisons/
        /// </remarks>

        (float Min, float Max) Get(float[] ticks);
        (float Min, float Max) GetMinMaxGain(string dailyStockPriceTicks);
    }
}
