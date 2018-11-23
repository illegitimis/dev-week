namespace Tests.Unit
{
    using WebApi.Controllers;
    using NSubstitute;
    using Xunit;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using DevWeek.Algo;
    using static Paths;

    public class MyControllerTest
    {
        readonly UploadController myController;
        readonly IFormFile formFile;
        readonly IProcessZip unzip;

        public MyControllerTest()
        {
            formFile = Substitute.For<IFormFile>();
            unzip = Substitute.For<IProcessZip>();

            myController = new UploadController(unzip);
        }

        [Fact(Skip = "old")]
        public async Task UploadsSingleFileOldOk()
        {
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms))
            {
                writer.Write("Hello World from a Fake File");
                writer.Flush();
                ms.Position = 0;
                formFile.OpenReadStream().Returns(ms);
                formFile.Length.Returns(ms.Length);

                var result = myController.UploadSingleFile(formFile);

                Assert.IsType<OkObjectResult>(result);
                await formFile.ReceivedWithAnyArgs().CopyToAsync(ms);
            }           
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UploadsSingleFileOk()
        {
            using (var fs = File.OpenRead($"{ZipsFolder}{InputsZip}"))
            {
                formFile.Length.Returns(fs.Length);
                formFile.OpenReadStream().Returns(fs);
                // unzip.GetMetadata(Arg.Any<Stream>()).ReturnsForAnyArgs(new[] { (fs.Name, fs.Length) });

                var result = await myController.UploadSingleFile(formFile);

                var ok = Assert.IsType<OkObjectResult>(result);
                /*
                var enumerable = (ok.Value as dynamic).metadata as IEnumerable<(string name, long length)>;
                var single = Assert.Single(enumerable);
                Assert.EndsWith(".zip", single.name);
                Assert.Equal(403, single.length);
                */
            }
        }
    }
}
