namespace Tests.Unit
{
    using DevWeek.Algo;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Xunit;
    using static Constants;

    public class StockPriceTickerTests
    {
        [Theory]
        [MemberData(nameof(StockPricePickerAlgorithms))]
        public void TenTicksArray(IPickStockPrice algo)
        {
            (float min, float max) = algo.Get(new float[] { 19.35f, 19.30f, 18.88f, 18.93f, 18.95f, 19.03f, 19.00f, 18.97f, 18.97f, 18.98f });
            Assert.Equal(18.88f, min);
            Assert.Equal(19.03f, max);
        }

        [Fact]
        public void OneHundredTicksPerformance()
        {
            var ticks = OneHundredTicks            
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(token => float.Parse(token))
                .ToArray();

            var performance = new Dictionary<IPickStockPrice, long>();

            foreach (var objects in StockPricePickerAlgorithms())
            {
                IPickStockPrice algo = (IPickStockPrice)objects[0];

                var stopwatch = Stopwatch.StartNew();
                (float min, float max) = algo.Get(ticks);
                stopwatch.Stop();
                performance.Add(algo, stopwatch.ElapsedTicks);

                Assert.Equal(8.03f, min);
                Assert.Equal(9.34f, max);
            }            
        }

        [Theory]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(5000)]
        [InlineData(10000)]
        [InlineData(50000)]
        [InlineData(100000)]
        [InlineData(500000)]
        [InlineData(1000000)]
        [InlineData(5000000)]
        [InlineData(10000000)]
        [InlineData(50000000)]
        [InlineData(100000000)]
        public void PerformanceComparison(int numberOfTicks)
        {
            const int largestMersennePrimeInteger = 2147483647;
            Random r = new Random(largestMersennePrimeInteger);

             var ticks = Enumerable
                .Range(1, numberOfTicks)
                .Select(x => (float)Math.Round(value: r.NextDouble(), digits: 2, mode: MidpointRounding.AwayFromZero))
                .ToArray();

            using (FileStream fs = File.OpenWrite("perf.csv"))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.BaseStream.Seek(0, SeekOrigin.End);

                foreach (var objects in StockPricePickerAlgorithms())
                {
                    IPickStockPrice algo = (IPickStockPrice)objects[0];

                    var stopwatch = Stopwatch.StartNew();
                    (float min, float max) = algo.Get(ticks);
                    stopwatch.Stop();

                    sw.WriteLine($"{algo.GetType().Name},{numberOfTicks},{stopwatch.ElapsedTicks}");

                    Assert.Equal(0f, min);
                    Assert.Equal(1f, max);
                }

            }                
        }

        public static IEnumerable<object[]> StockPricePickerAlgorithms()
        {
            yield return new object[] { new ParallelForStockPricePicker() };
            yield return new object[] { new NaiveStockPricePicker() };
            yield return new object[] { new NoInliningNaiveStockPricePicker() };
            yield return new object[] { new NoOptimizationNaiveStockPricePicker() };
            yield return new object[] { new AggressiveInliningNaiveStockPricePicker() };
            yield return new object[] { new BetterStockPricePicker() };
            yield return new object[] { new NoInliningBetterStockPricePicker() };
            yield return new object[] { new NoOptimizationBetterStockPricePicker() };
            yield return new object[] { new AggressiveInliningBetterStockPricePicker() };            
        }

    }
}
