
namespace Tests.Integration
{
    public sealed class RestSharpLocalTest : RestSharpBaseTest
    {
        internal override string EndPoint() => "http://localhost:5000";
    }
}
