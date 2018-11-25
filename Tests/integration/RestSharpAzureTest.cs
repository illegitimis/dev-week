
namespace Tests.Integration
{
    using Xunit;

    public sealed class RestSharpAzureTest : RestSharpBaseTest 
    {
        public RestSharpAzureTest() : base() { }

        internal override string EndPoint() => "http://acp-dev-week.azurewebsites.net/";

        [Fact]
        public void AzureStreaming ()
        {
             /*
              * const string boundary = "----WebKitFormBoundary7MA4YWxkTrZu0gW";

            var client = new RestClient("http://acp-dev-week.azurewebsites.net/streaming");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            var content = $"multipart/form-data; boundary={boundary}";
            request.AddHeader("content-type", content);
            // ..\\..\\..\\..\\zips\\
            // request.AddParameter(content, $"{boundary}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"inputs.zip\"\r\nContent-Type: application/zip\r\n\r\n\r\n--{boundary}--", ParameterType.RequestBody);
           
            request.AddFile(InputsZip, InputsZip);
          
            
            client.ExecuteAsyncPost(request, (IRestResponse response, RestRequestAsyncHandle requestAsyncHandle) => 
            {

            }, "POST");
            */
        }
    }
}
