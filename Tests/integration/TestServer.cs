namespace Tests.Integration
{
    using Microsoft.AspNetCore.TestHost;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using WebApi;
    using Xunit;
    using static Paths;

    public sealed class TestServer : IDisposable
    {
        readonly Microsoft.AspNetCore.TestHost.TestServer testServer;
        readonly HttpClient apiClient;

        public TestServer()
        {
            var webHostBuilder = Program.CreateWebHostBuilder(new string[0]);
            testServer = new Microsoft.AspNetCore.TestHost.TestServer(webHostBuilder);
            apiClient = testServer.CreateClient();
        }

        /// <summary>https://rules.sonarsource.com/csharp/RSPEC-3881</summary>
        public void Dispose()
        {
            apiClient.Dispose();
            testServer.Dispose();
        }

        [Fact]
        public void TestServerCreation()
        {
            Assert.NotNull(testServer);
            Assert.NotNull(apiClient);
        }


        [Fact(Skip = "Unexpected end of Stream, the content may have already been read by another component.")]
        public async Task PostStreaming()
        {
            string fileName = $"{ZipsFolder}{InputsZip}";
            //using (var fs = File.OpenRead(fileName))
            //using (var sr = new StreamReader(fs))
            {
                // StreamContent content = new StreamContent(fs);
                var bytes = await File.ReadAllBytesAsync(fileName);
                ByteArrayContent content = new ByteArrayContent(bytes);
                // !! application/x-www-form-urlencoded !!
                const string MediaTypeMultipartFormData = "multipart/form-data";
                const string boundary = "boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW";
                string contentType = $"{MediaTypeMultipartFormData}; {boundary}";
                // The format of value 'multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW' is invalid.
                // content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                content.Headers.Remove("Content-Type");
                content.Headers.TryAddWithoutValidation("Content-Type", contentType);

                // act
                using (var httpResponseMessage = await apiClient.PostAsync("streaming", content))
                {
                    // assert status
                    Assert.NotNull(httpResponseMessage);
                    Assert.False(httpResponseMessage.IsSuccessStatusCode);
                    Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);

                    // Assert response body
                    var body = await httpResponseMessage.Content.ReadAsStringAsync();
                    Assert.Equal("{\"\":[\"A non-empty request body is required.\"]}", body);
                }
            }
        }
    }
}
