
namespace Tests.Integration
{
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using WebApi.Dto;
    using Xunit;
    using static Paths;

    public abstract class RestSharpBaseTest : IDisposable
    {
        protected readonly RestClient client;

        protected RestSharpBaseTest()
        {
            client = new RestClient(EndPoint());
            client.AddHandler("application/json; charset=utf-8", NewtonsoftJsonDeSerializer.Default);
            client.AddHandler("application/json", NewtonsoftJsonDeSerializer.Default);
        }

        internal abstract string EndPoint();

        public void Dispose()
        {
            // client dispose
        }

        [Fact]
        public void PingsEndpoint()
        {
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Get(request);
            Assert.NotNull(response);
            Assert.Equal("OK", response.StatusDescription);
            Assert.Contains("Ping", response.Content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PingsEndpointAsync()
        {
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteGetTaskAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void PostsMultipartFormDataSuccessfully()
        {
            var request = new RestRequest("streaming", Method.POST);
            request.AddFile(name: "file", path: $"{ZipsFolder}{InputsZip}", contentType: "multipart/form-data");
            IRestResponse response = client.Post(request);
            Assert.NotNull(response);
            Assert.Equal("OK", response.StatusDescription);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.ContentType);
        }

        [Theory]
        [InlineData("streaming")]
        [InlineData("upload")]
        public async Task PostsMultipartFormDataAndDeserializesAsync(string controllerName)
        {
            var request = new RestRequest(controllerName, Method.POST);
            request.AddFile(name: "file", path: $"{ZipsFolder}{InputsZip}", contentType: "multipart/form-data");
            IRestResponse<UploadResponseDto> response = await client.ExecutePostTaskAsync<UploadResponseDto> (request);
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.Items.Count);
        }
    }
}
