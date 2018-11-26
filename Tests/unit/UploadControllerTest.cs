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
    using System.Collections.Generic;
    using WebApi.Dto;

    public class UploadControllerTest
    {
        readonly UploadController myController;
        readonly IFormFile formFile;
        readonly IProcessZip unzip;

        public UploadControllerTest()
        {
            formFile = Substitute.For<IFormFile>();
            unzip = Substitute.For<IProcessZip>();
            myController = new UploadController(unzip);
        }

       
        [Fact]
        public async Task UploadsSingleFileOk()
        {
            using (var fs = File.OpenRead($"{ZipsFolder}{InputsZip}"))
            {
                formFile.Length.Returns(fs.Length);
                formFile.OpenReadStream().Returns(fs);
                unzip.ProcessAsync(Arg.Any<MemoryStream>()).ReturnsForAnyArgs(new List<ProcessZipItemModel>(new[] { new ProcessZipItemModel(0, 1, "name") }));
                
                var result = await myController.UploadSingleFile(formFile);

                var ok = Assert.IsType<OkObjectResult>(result);
                var dto = Assert.IsType<UploadResponseDto>(ok.Value);
                var item = Assert.Single(dto.Items);
                Assert.Equal(0, item.BuyPoint);
                Assert.Equal(1, item.SellPoint);
                Assert.Equal("name", item.FileName);                
            }
        }
    }
}
