namespace Tests.Unit
{
    using DevWeek.Algo;
    using System.Linq;
    using Xunit;
    using static Paths;

    public class UnzipperTest
    {
        /// <summary>
        /// TODO
        /// </summary>
        [Fact]
        public void GetFileMetadataTest()
        {
            var unzip = new HangfireLikeZipProcessor(null, null);
            /*
            var results = unzip.GetMetadata($"{ZipsFolder}{InputsZip}").ToArray();
            Assert.Equal(2, results.Length);
            Assert.Equal($"{PicsFolder}{TenTicksPng}", results[0].name);
            Assert.Equal(403, results[0].length);
            Assert.Equal($"{PicsFolder}{HundredTicksPng}", results[1].name);
            Assert.Equal(1055, results[1].length);
            */
        }
    }
}
