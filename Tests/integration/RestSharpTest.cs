
namespace Tests.Integration
{
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using static Paths;

    public abstract class RestSharpBaseTest : IDisposable
    {
        protected readonly RestClient client;

        protected RestSharpBaseTest()
        {
            client = new RestClient(EndPoint());
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
    }
}
