namespace WebApi.Controllers
{
    using DevWeek.Algo;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using WebApi.Dto;

    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-2.1#uploading-small-files-with-model-binding
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        readonly IUnzip unzipper;

        public UploadController(IUnzip unzip)
        {
            unzipper = unzip ?? throw new ArgumentNullException(nameof(unzip));            
        }

        // POST https://acp-dev-week.azurewebsites.net/upload
        [HttpPost]
        public async Task<IActionResult> UploadSingleFile(IFormFile file)
        {
            if (file == null) return BadRequest("null form file");
            if (file.Length == 0) return BadRequest("empty form file");

            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var completedTasksResponses = await unzipper.Process(memoryStream);
            var dto = new UploadResponseDto(completedTasksResponses);

            memoryStream.Close();

            return Ok(dto);
        }

        // POST api/my/persist
        [Obsolete("debug purposes")]
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
