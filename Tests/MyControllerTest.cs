namespace Tests
{

using System;
using System.Collections.Generic;
using System.Text;
    using WebApi.Controllers;
    using NSubstitute;
    using Xunit;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using DevWeek.Algo;
    using static Constants;
    using System.Linq;

    public class MyControllerTest
    {
        readonly MyController myController;
        readonly IFormFile formFile;
        readonly IUnzip unzip;

        public MyControllerTest()
        {
            formFile = Substitute.For<IFormFile>();
            unzip = Substitute.For<IUnzip>();

            myController = new MyController(unzip, null, null);
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

        [Fact]
        public async Task UploadsSingleFileOk()
        {
            using (var fs = File.OpenRead(InputsZipFileName))
            {
                formFile.Length.Returns(fs.Length);
                formFile.OpenReadStream().Returns(fs);
                unzip.GetMetadata(Arg.Any<Stream>()).ReturnsForAnyArgs(new[] { (fs.Name, fs.Length) });

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
