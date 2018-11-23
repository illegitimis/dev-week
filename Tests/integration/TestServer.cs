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

        [Fact]
        public async Task StreamingPost()
        {
            // arrange
            StreamContent content = null;
            using (var fs = File.OpenRead($"{ZipsFolder}{InputsZip}"))
            {
                content = new StreamContent(fs);
                // !! application/x-www-form-urlencoded !!
                content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

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
