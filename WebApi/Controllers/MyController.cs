namespace WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using DevWeek.Algo;
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using WebApi.Dto;

    [Route("dev-week")]
    [ApiController]
    public class MyController : ControllerBase
    {
        readonly IUnzip unzipper;

        readonly IReadQrCode qrCodeReader;

        readonly IPickStockPrice stockPricePicker;

        public MyController(IUnzip unzip, IReadQrCode readQrCode, IPickStockPrice pickStockPrice)
        {
            unzipper = unzip ?? throw new ArgumentNullException(nameof(unzip));
            qrCodeReader = readQrCode ?? throw new ArgumentNullException(nameof(qrCodeReader));
            stockPricePicker = pickStockPrice ?? throw new ArgumentNullException(nameof(stockPricePicker));
        }

        // POST api/my/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadSingleFile(IFormFile zipFormFile)
        {
            if (zipFormFile == null) return BadRequest("null form file");
            if (zipFormFile.Length == 0) return BadRequest("empty form file");

            var memoryStream = new MemoryStream();
            await zipFormFile.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var completedTasksResponses = await unzipper.Process(memoryStream);
            var dto = new UploadResponseDto(completedTasksResponses);

            memoryStream.Close();

            return Ok(dto);
        }

        // POST api/my/persist
        [HttpPost("persist")]
        public async Task<IActionResult> PersistSingleFile(IFormFile zipFormFile)
        {
            if (zipFormFile == null) return BadRequest("null form file");
            if (zipFormFile.Length == 0) return BadRequest("empty form file");

            // opens the request stream for reading uploaded file
            // var stream = formFile.OpenReadStream();

            using (var stream = new FileStream("filePath.zip", FileMode.Create))
            {
                await zipFormFile.CopyToAsync(stream);
                return Ok();
            }
        }
    }
}
