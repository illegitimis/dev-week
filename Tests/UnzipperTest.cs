namespace Tests
{
    using DevWeek.Algo;
    using System.Linq;
    using Xunit;
    using static Constants;

    public class UnzipperTest
    {
        [Fact]
        public void GetFileMetadataTest()
        {
            var unzip = new Unzipper(null);
            var results = unzip.GetMetadata(InputsZipFileName).ToArray();
            Assert.Equal(2, results.Length);
            Assert.Equal(FileNameTenTicks, results[0].name);
            Assert.Equal(403, results[0].length);
            Assert.Equal(FileNameHundredTicks, results[1].name);
            Assert.Equal(1055, results[1].length);
        }
    }
}
