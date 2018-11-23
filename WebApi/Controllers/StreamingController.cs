namespace WebApi.Controllers
{
    using DevWeek.Algo;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using Microsoft.Net.Http.Headers;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApi.Dto;
    using WebApi.Extensions;
    using WebApi.Filters;
    using File = System.IO.File;

    [Route("/[controller]")]
    [ApiController]
    public class StreamingController : ControllerBase
    {
        private readonly IProcessZip _zipProcessor;
        private readonly ILogger<StreamingController> _logger;

        // Get the default form options so that we can use them to set the default limits for
        // request body data
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public StreamingController(IProcessZip zipProcessor, ILogger<StreamingController> logger)
        {
            _logger = logger;
            _zipProcessor = zipProcessor;
        }

        [HttpGet]
        [GenerateAntiforgeryTokenCookieForAjax]
        public IActionResult Index() => Ok("Ping");

        #region snippet1
        // 1. Disable the form value model binding here to take control of handling potentially large files.
        // 2. Typically antiforgery tokens are sent in request body, but since we 
        //    do not want to read the request body early, the tokens are made to be 
        //    sent via headers. The antiforgery token filter first looks for tokens
        //    in the request header and then falls back to reading the body.
        [HttpPost]
        [DisableFormValueModelBinding]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Upload(MultipartModel model)
        public async Task<IActionResult> Upload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            // Used to accumulate all the form url encoded key value pairs in the 
            // request.
            var formAccumulator = new KeyValueAccumulator();
            UploadResponseDto dto = null;

            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            
            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    _logger.LogDebug($"{nameof(hasContentDispositionHeader)} {section.ContentDisposition}");

                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        var memoryStream = new MemoryStream();
                        await section.Body.CopyToAsync(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        var completedTasksResponses = await _zipProcessor.ProcessAsync(memoryStream);
                        dto = new UploadResponseDto(completedTasksResponses);
                        memoryStream.Close();
                        _logger.LogInformation($"File content disposition '{contentDisposition}'");
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        // Content-Disposition: form-data; name="key"; value
                        // Do not limit the key name length here because the multipart headers length limit is already in effect.
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
                        var encoding = section.GetEncoding();
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key, value);

                            if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            // Bind form data to a model
            // InvalidOperationException: Method may only be called on a Type for which Type.IsGenericParameter is true.

            /*
            var fields = formAccumulator.GetResults();
            var formValueProvider = new FormValueProvider(BindingSource.Form, new FormCollection(fields), CultureInfo.CurrentCulture);
            
            var bindingSuccessful = await TryUpdateModelAsync<UploadResponseDto>(dto, prefix: "", valueProvider: formValueProvider);
            if (!bindingSuccessful && !ModelState.IsValid) return BadRequest(ModelState);
            */
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(dto);
        }
        #endregion
        
    }
}