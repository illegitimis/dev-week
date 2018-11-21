namespace Tests
{
    using System;
    using Xunit;
    using DevWeek.Algo;

    public class FibonacciPQTest
    {
        [Fact]
        public void Test1()
        {
            // 19.35 19.30 18.88 18.93 18.95 19.03 19.00 18.97 18.97 18.98
            // fpq.Insert(10, f);

            var fpq = new FibonacciPQ<float>(10);
            
            fpq.Insert(0, 19.35f);
            fpq.Insert(1, 19.30f);
            fpq.Insert(2, 18.88f);
            fpq.Insert(3, 18.93f);
            fpq.Insert(4, 18.95f);
            fpq.Insert(5, 19.03f);
            fpq.Insert(6, 19.00f);
            fpq.Insert(7, 18.97f);
            fpq.Insert(8, 18.97f);
            fpq.Insert(9, 18.98f);

            fpq.Consolidate();

            // Output: 18.88 19.03
            Assert.Equal(18.88f, fpq.MinimumKey());
            Assert.Equal(19.35f, fpq.MaximumKey());
            // obviously I can't buy in the future and sell in the past.
            Assert.Equal(19.03f, fpq.MaximumKey());
        }
    }
}
