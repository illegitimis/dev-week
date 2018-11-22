namespace WebApi.Dto
{
using Microsoft.AspNetCore.Http;

    public class MultipartModel
    {
        public IFormFile File { get; set; }
    }
}