namespace WebApi.Extensions
{

using Microsoft.Net.Http.Headers;
using System;
using System.IO;


    public static class MultipartRequestHelper
    {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            if (boundary.IsNullOrWhiteSpace())
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary.Value;
        }

        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

            // Content-Disposition: form-data; name="key";
        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && contentDisposition.FileName.IsNullOrEmpty()
                   && contentDisposition.FileNameStar.IsNullOrEmpty();
        }

            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && (!contentDisposition.FileName.IsNullOrEmpty()
                       || !contentDisposition.FileNameStar.IsNullOrEmpty());
        }
    }
}
