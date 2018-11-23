namespace WebApi.Dto
{
using Microsoft.AspNetCore.Http;

    /// <summary>
    /// <see cref="IFormFile"/> wrapper
    /// </summary>
    /// <remarks>unused</remarks>
    public class MultipartModel
    {
        public IFormFile File { get; set; }
    }
}