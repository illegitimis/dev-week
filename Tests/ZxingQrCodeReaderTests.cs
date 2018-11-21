namespace Tests
{
using DevWeek.Algo;
using System;
using System.Collections.Generic;
using System.Text;
    using Xunit;

    using static Constants;

    public class ZxingQrCodeReaderTests
    {
        private readonly ZxingQrCodeReader zxingQrCodeReader;

        public ZxingQrCodeReaderTests()
        {
            zxingQrCodeReader = new ZxingQrCodeReader();
        }

        [Fact]
        public void one()
        {
            var stringResult = zxingQrCodeReader.DecodePngFile("../../../../pics/sample-input-1.png");
            Assert.Equal(TenTicks, stringResult);
        }

        [Fact]
        public void two()
        {
            var stringResult = zxingQrCodeReader.DecodePngFile("../../../../pics/sample-input-2.png");
            Assert.Equal(OneHundredTicks, stringResult);
        }
    }
}
