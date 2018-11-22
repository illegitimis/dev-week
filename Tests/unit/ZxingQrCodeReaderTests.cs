namespace Tests.Unit
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
            var stringResult = zxingQrCodeReader.DecodePngFile($"../../../../pics/{FileNameTenTicks}");
            Assert.Equal(TenTicks, stringResult);
        }

        [Fact]
        public void two()
        {
            var stringResult = zxingQrCodeReader.DecodePngFile($"../../../../pics/{FileNameHundredTicks}");
            Assert.Equal(OneHundredTicks, stringResult);
        }
    }
}
