
namespace Tests.Integration
{
    using Xunit;

    public sealed class RestSharpAzureTest : RestSharpBaseTest 
    {
        public RestSharpAzureTest() : base() { }

        internal override string EndPoint() => "http://acp-dev-week.azurewebsites.net/";        
    }
}
